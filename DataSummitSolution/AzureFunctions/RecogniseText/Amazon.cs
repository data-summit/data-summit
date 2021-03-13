using Amazon.Rekognition.Model;
using Amazon.Rekognition;
using AWS = Amazon;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.Methods.PostProcessing;
using DataSummitModels.Cloud.Consolidated;

namespace AzureFunctions.RecogniseText
{
    public static class Amazon
    {
        [FunctionName("RecogniseTextAmazon")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                string name = req.Query["name"];

                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<DataSummitModels.DB.ImageUpload>(jsonContent);
                DataSummitModels.DB.ImageUpload imgUp = (DataSummitModels.DB.ImageUpload)data;

                //Validate entry data
                if (imgUp.FileName == "") return new BadRequestObjectResult("Illegal input: File name is ,less than zero.");
                //if (imgUp.Type == DataSummitModels.Enums.Document.Type.Unknown) return new BadRequestObjectResult("Illegal input: Type is blank.");
                if (imgUp.StorageAccountName == "") return new BadRequestObjectResult("Illegal input: Storage name required.");
                if (imgUp.StorageAccountKey == "") return new BadRequestObjectResult("Illegal input: Storage key required.");
                if (imgUp.WidthOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have width greater than zero");
                if (imgUp.HeightOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have height greater than zero");
                if (imgUp.ContainerName == "") return new BadRequestObjectResult("Illegal input: Container must have a GUID name");
                if (imgUp.SplitImages == null) return new BadRequestObjectResult("Illegal input: 'SplitImages' list is null");
                if (imgUp.SplitImages.Count == 0) return new BadRequestObjectResult("Illegal input: 'SplitImages' list is empty");
                if (imgUp.Tasks == null) return new BadRequestObjectResult("Illegal input: 'Tasks' list is null");
                if (imgUp.Tasks.Count == 0) return new BadRequestObjectResult("Illegal input: 'Tasks' list is empty");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.StorageAccountName +
                                           ";AccountKey=" + imgUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account == null) { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                imgUp.Tasks.Add(new DataSummitModels.DB.Tasks("Amazon OCR\tGet container", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (await cbc.ExistsAsync() == false) return new BadRequestObjectResult("Illegal input: Cannot find Container with name: " + imgUp.ContainerName);

                //Write new or append OCR results to separete files
                //string sOCRRes = "All OCR Results.json";
                //CloudBlockBlob jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);

                AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Keys.Amazon.AccessKeyID, Keys.Amazon.SecretAccessKey, AWS.RegionEndpoint.EUWest1);
                DetectTextRequest detectTextRequest = new DetectTextRequest();

                List<Task> lOCRTasks = new List<Task>();

                foreach (DataSummitModels.DB.ImageGrids ig in imgUp.SplitImages)
                {
                    //lOCRTasks.Add(Task.Run(() =>
                    //{
                    //Download blob image data
                    Uri uriBlob = new Uri(ig.BlobUrl);
                    CloudBlockBlob blockBlobOrig = new CloudBlockBlob(uriBlob);
                    MemoryStream ms = new MemoryStream();
                    await blockBlobOrig.DownloadToStreamAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    detectTextRequest.Image = new Image();
                    detectTextRequest.Image.Bytes = ms;
                    DetectTextResponse res = await rekognitionClient.DetectTextAsync(detectTextRequest);
                    List<Sentences> sentences = new List<Sentences>();

                    if (res != null)
                    {
                        Methods.OCRSources ocrMethods = new Methods.OCRSources();
                        sentences.AddRange((IEnumerable<Sentences>)ocrMethods.FromAmazon(res.TextDetections, ig));

                        if (ig.Sentences == null) ig.Sentences = new List<DataSummitModels.DB.Sentences>();
                        //foreach(Sentences s in )
                        ig.Sentences.AddRange(Methods.WordLocation.Corrected(sentences, ig));

                        imgUp.Tasks.Add(new DataSummitModels.DB.Tasks("Amazon OCR\tUnified image " + imgUp.SplitImages.IndexOf(ig).ToString("000") + " results", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                        log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ": " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());
                    }
                    //}));
                }

                imgUp.Tasks.Add(new DataSummitModels.DB.Tasks("Amazon OCR\tAll OCR tasks started", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Task.WaitAll(lOCRTasks.ToArray());

                imgUp.Tasks.Add(new DataSummitModels.DB.Tasks("Amazon OCR\tAll OCR tasks finished", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Extract sentences from each ImageGrid and consolidate into ImageUpload (technical duplicate)
                if (imgUp.Sentences == null) imgUp.Sentences = new List<DataSummitModels.DB.Sentences>();
                foreach (DataSummitModels.DB.ImageGrids ig in imgUp.SplitImages)
                {
                    imgUp.Sentences.AddRange(ig.Sentences);
                    ig.Sentences = null;
                }

                Self self = new Self();
                { }
                //TODO verify which class is required
                //List<DataSummitModels.DB.Sentences> lResults = self.Clean(imgUp
                //                            .Sentences.Select(s => s.ToModelConsolidated()).ToList())
                //                            .Select(s => s.ToModel()).ToList();
                List<DataSummitModels.DB.Sentences> lResults = new List<DataSummitModels.DB.Sentences>();
                imgUp.Sentences = lResults.Where(s => s.IsUsed == true).ToList();

                //if (jsonSentencesBlob.Exists() == true)
                //{
                //    imgUp.Sentence.AddRange(JsonConvert.DeserializeObject<List<Sentence>>(jsonSentencesBlob.DownloadText()));
                //    jsonSentencesBlob.Delete();
                //}
                //jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);
                //jsonSentencesBlob.UploadText(JsonConvert.SerializeObject(lResults));

                imgUp.Tasks.Add(new DataSummitModels.DB.Tasks("Amazon OCR\t'All OCR Results' uploaded", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

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
