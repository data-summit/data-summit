using DataSummitModels.DTO;
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
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions.Document
{
    public static class Upload
    {
        [FunctionName("Upload")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<DocumentUpload>(jsonContent);
                DocumentUpload docUp = (DocumentUpload)data;

                List<Task> lTasks = new List<Task>();

                if (jsonContent.Length == 0) return new BadRequestObjectResult("Illegal input: No content");
                if (docUp.File.Length == 0) return new BadRequestObjectResult("Illegal input: PDF/Image is empty.");
                if (docUp.StorageAccountName == "") return new BadRequestObjectResult("Illegal input: Storage name required.");
                if (docUp.StorageAccountKey == "") return new BadRequestObjectResult("Illegal input: Storage key required.");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + docUp.StorageAccountName +
                                           ";AccountKey=" + docUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Storage account connection";
                if (account == null) { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                //Initiate Azure container object
                docUp.ContainerName = Guid.NewGuid().ToString();
                CloudBlobContainer cbc = blobClient.GetContainerReference(docUp.ContainerName);

                //Create container if it doesn't exist
                await cbc.CreateIfNotExistsAsync();
                BlobContainerPermissions permissions = await cbc.GetPermissionsAsync();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                await cbc.SetPermissionsAsync(permissions);

                //Add event checking that whether it is the first event to exist in list
                if (docUp.Tasks.Count == 0)
                { docUp.Tasks.Add(new Tasks("Container created", DateTime.Now)); }
                else
                { docUp.Tasks.Add(new Tasks("Container created", docUp.Tasks[docUp.Tasks.Count - 1].TimeStamp)); }
                log.LogInformation(docUp.Tasks[docUp.Tasks.Count - 1].Name);

                CloudBlockBlob cbbImage = cbc.GetBlockBlobReference("Original.jpg");
                lTasks.Add(Task.Run(async () =>
                {
                    AccessCondition ac = new AccessCondition();
                    BlobRequestOptions brq = new BlobRequestOptions();
                    OperationContext oc = new OperationContext();
                    oc.LogLevel = Microsoft.WindowsAzure.Storage.LogLevel.Informational;
                    await cbbImage.UploadFromStreamAsync(docUp.File.OpenReadStream(), ac, brq, oc);
                    //Export image to blockBlob
                    cbbImage.Metadata.Add("CompanyId", docUp.CompanyId.ToString());
                    cbbImage.Metadata.Add("UserId", docUp.UserId.ToString());
                    cbbImage.Metadata.Add("FileName", docUp.File.FileName.ToString());
                    cbbImage.Metadata.Add("DocumentFormat", docUp.DocumentFormat.ToString());
                    cbbImage.Metadata.Add("DocumentType", docUp.DocumentType.ToString());
                    cbbImage.Metadata.Add("PaymentPlan", docUp.PaymentPlan.ToString());
                    await cbbImage.SetMetadataAsync();

                    docUp.Tasks.Add(new Tasks("Image uploaded to blob", docUp.Tasks[docUp.Tasks.Count - 1].TimeStamp));
                    log.LogInformation(docUp.Tasks[docUp.Tasks.Count - 1].Name);

                    //Clear heavy payload content
                    docUp.File = null;
                    docUp.IsUploaded = true;
                    docUp.BlobUrl = cbbImage.Uri.ToString();
                }));

                Task.WaitAll(lTasks.ToArray());

                //Return single image object
                string jsonToReturn = JsonConvert.SerializeObject(docUp);
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