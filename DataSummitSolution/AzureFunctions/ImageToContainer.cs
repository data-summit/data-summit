using DataSummitModels.DB;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public static class ImageToContainer
    {
        [FunctionName("ImageToContainer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string name = req.Query["name"];

                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;

                List<Task> lTasks = new List<Task>();

                if (imgUp.Tasks == null) imgUp.Tasks = new List<Tasks>();
                if (imgUp.Layers == null) imgUp.Layers = new List<DocumentLayers>();

                if (jsonContent.Length == 0) return new BadRequestObjectResult("Illegal input: No content");
                //if (imgUp.CompanyId < 0) return new BadRequestObjectResult("Illegal input: CompanyId is less than zero.");
                //if (imgUp.ProjectId < 0) return new BadRequestObjectResult("Illegal input: ProjectId is less than zero.");
                if (imgUp.DocumentId < 0) return new BadRequestObjectResult("Illegal input: DocumentId is less than zero.");
                //if (imgUp.Company == "") return new BadRequestObjectResult("Illegal input: Company is blank.");
                //if (imgUp.Project == "") return new BadRequestObjectResult("Illegal input: Project is blank.");
                if (imgUp.FileName == null) return new BadRequestObjectResult("Illegal input: File name is empty.");
                //if (imgUp.Type == DataSummitModels.Enums.Document.Type.Unknown) return new BadRequestObjectResult("Illegal input: Type is blank.");
                if (imgUp.File.Length == 0) return new BadRequestObjectResult("Illegal input: PDF/Image is empty.");
                if (imgUp.StorageAccountName == "") return new BadRequestObjectResult("Illegal input: Storage name required.");
                if (imgUp.StorageAccountKey == "") return new BadRequestObjectResult("Illegal input: Storage key required.");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.StorageAccountName +
                                           ";AccountKey=" + imgUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Storage account connection";
                if (account == null) { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                //Initiate Azure container object
                imgUp.ContainerName = Guid.NewGuid().ToString();
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);

                //Create container if it doesn't exist
                await cbc.CreateIfNotExistsAsync();
                BlobContainerPermissions permissions = await cbc.GetPermissionsAsync();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                await cbc.SetPermissionsAsync(permissions);

                //Add event checking that whether it is the first event to exist in list
                if (imgUp.Tasks.Count == 0)
                { imgUp.Tasks.Add(new Tasks("Container created", DateTime.Now)); }
                else
                { imgUp.Tasks.Add(new Tasks("Container created", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp)); }
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);

                CloudBlockBlob cbbImage = cbc.GetBlockBlobReference("Original.jpg");
                lTasks.Add(Task.Run(async () =>
                {
                    await cbbImage.UploadFromByteArrayAsync(imgUp.File, 0, imgUp.File.Length);
                    //Export image to blockBlob
                    cbbImage.Metadata.Add("CompanyId", imgUp.CompanyId.ToString());
                    cbbImage.Metadata.Add("ProjectId", imgUp.ProjectId.ToString());
                    cbbImage.Metadata.Add("FileName", imgUp.FileName.ToString());
                    cbbImage.Metadata.Add("Type", imgUp.Type.ToString());
                    cbbImage.Metadata.Add("Width", imgUp.WidthOriginal.ToString());
                    cbbImage.Metadata.Add("Height", imgUp.HeightOriginal.ToString());
                    await cbbImage .SetMetadataAsync();

                    imgUp.Tasks.Add(new Tasks("Image uploaded to blob", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                    log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);

                    //Clear heavy payload content
                    imgUp.File = null;
                    imgUp.Processed = true;
                    imgUp.BlobUrl = cbbImage.Uri.ToString();
                }));

                lTasks.Add(Task.Run(() =>
                {
                    //Image bmp = (Bitmap)((new ImageConverter()).ConvertFrom(imgUp.File));
                    using (var ms = new MemoryStream(imgUp.File))
                    {
                        Image bmp = new Bitmap(ms);
                        imgUp.WidthOriginal = bmp.Width;
                        imgUp.HeightOriginal = bmp.Height;

                        imgUp.Tasks.Add(new Tasks("Image dimensions assessed", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                        log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);
                    }
                }));

                Task.WaitAll(lTasks.ToArray());

                //Return single image object
                string jsonToReturn = JsonConvert.SerializeObject(imgUp);
                return new OkObjectResult(jsonToReturn);
            }
            catch (Exception ae)
            {
                //return error generated within function code
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ae));
            }
        }
    }
}