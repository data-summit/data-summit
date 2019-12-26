using DataSummitFunctions.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;

using SelectPdf;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions
{
    public static class ImageToContainer
    {
        [FunctionName("ImageToContainer")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                                            HttpRequestMessage req, TraceWriter log)// Microsoft.Extensions.Logging.ILogger log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;

                List<Task> lTasks = new List<Task>();

                if (imgUp.Tasks == null) imgUp.Tasks = new List<Tasks>();
                if (imgUp.Layers == null) imgUp.Layers = new List<string>();

                if (jsonContent.Length == 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: No content");
                //if (imgUp.CompanyId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: CompanyId is less than zero.");
                //if (imgUp.ProjectId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: ProjectId is less than zero.");
                if (imgUp.DrawingId < 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: DrawingId is less than zero.");
                //if (imgUp.Company == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Company is blank.");
                //if (imgUp.Project == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Project is blank.");
                if (imgUp.FileName == null) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: File name is empty.");
                if (imgUp.Type == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Type is blank.");
                if (imgUp.File.Length == 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: PDF/Image is empty.");
                if (imgUp.BlobStorageName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage name required.");
                if (imgUp.BlobStorageKey == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage key required.");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.BlobStorageName +
                                           ";AccountKey=" + imgUp.BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Storage account connection";
                if (account == null) { log.Info(strError + ": failed"); }
                else { log.Info(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.Info(strError + ": failed"); }
                else { log.Info(strError + " = " + blobClient.ToString() + ": success"); }

                //Initiate Azure container object
                imgUp.ContainerName = Guid.NewGuid().ToString();
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);

                //Create container if it doesn't exist
                cbc.CreateIfNotExists();
                BlobContainerPermissions permissions = cbc.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                cbc.SetPermissions(permissions);

                //Add event checking that whether it is the first event to exist in list
                if (imgUp.Tasks.Count == 0)
                { imgUp.Tasks.Add(new Tasks("Container created", DateTime.Now)); }
                else
                { imgUp.Tasks.Add(new Tasks("Container created", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp)); }
                log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);

                CloudBlockBlob cbbImage = cbc.GetBlockBlobReference("Original.jpg");
                lTasks.Add(Task.Run(() =>
                {
                    cbbImage.UploadFromByteArray(imgUp.File, 0, imgUp.File.Length);
                    //Export image to blockBlob
                    cbbImage.Metadata.Add("CompanyId", imgUp.CompanyId.ToString());
                    cbbImage.Metadata.Add("ProjectId", imgUp.ProjectId.ToString());
                    cbbImage.Metadata.Add("FileName", imgUp.FileName.ToString());
                    cbbImage.Metadata.Add("Type", imgUp.Type.ToString());
                    cbbImage.Metadata.Add("Width", imgUp.WidthOriginal.ToString());
                    cbbImage.Metadata.Add("Height", imgUp.HeightOriginal.ToString());
                    cbbImage.SetMetadataAsync();

                    imgUp.Tasks.Add(new Tasks("Image uploaded to blob", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                    log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);

                    //Clear heavy payload content
                    imgUp.File = null;
                    imgUp.Processed = true;
                    imgUp.BlobURL = cbbImage.Uri.ToString();
                }));

                lTasks.Add(Task.Run(() =>
                {
                    Image bmp = (Bitmap)((new ImageConverter()).ConvertFrom(imgUp.File));
                    imgUp.WidthOriginal = bmp.Width;
                    imgUp.HeightOriginal = bmp.Height;

                    imgUp.Tasks.Add(new Tasks("Image dimensions assessed", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                    log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name);
                }));

                Task.WaitAll(lTasks.ToArray());

                //Return single image object
                string jsonToReturn = JsonConvert.SerializeObject(imgUp);
                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };
            }
            catch (Exception ae)
            {
                //return error generated within function code
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
        }
    }
}