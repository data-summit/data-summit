using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using DataSummitModels.DB;
using DataSummitModels.Cloud.Consolidated;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using AzureFunctions.Models.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using AzureFunctions.Methods.PostProcessing;

namespace AzureFunctions
{
    public static class RecogniseTextAzure
    {
        [FunctionName("RecogniseTextAzure")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];

            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent,
                        new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTime });
                ImageUpload imgUp = (ImageUpload)data;
                name = name ?? data?.name;

                //Validate entry data
                if (imgUp.FileName == "") return new BadRequestObjectResult("Illegal input: File name is ,less than zero.");
                if (imgUp.Type == "") return new BadRequestObjectResult("Illegal input: Type is blank.");
                if (imgUp.StorageAccountName == "") return new BadRequestObjectResult("Illegal input: Storage name required.");
                if (imgUp.StorageAccountKey == "") return new BadRequestObjectResult("Illegal input: Storage key required.");
                if (imgUp.WidthOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have width greater than zero");
                if (imgUp.HeightOriginal <= 0) return new BadRequestObjectResult("Illegal input: Image must have height greater than zero");
                if (imgUp.ContainerName == "") return new BadRequestObjectResult("Illegal input: Container must have a GUID name");
                if (imgUp.SplitImages == null) return new BadRequestObjectResult("Illegal input: 'SplitImages' list is null");
                if (imgUp.SplitImages.Count == 0) return new BadRequestObjectResult("Illegal input: 'SplitImages' list is empty");
                if (imgUp.Tasks == null) return new BadRequestObjectResult("Illegal input: 'Tasks' list is null");
                if (imgUp.Tasks.Count == 0) return new BadRequestObjectResult("Illegal input: 'Tasks' list is empty");

                List<BlobOCR> lRes = new List<BlobOCR>();
                Drawings ocrRes = new Drawings();

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.StorageAccountName +
                                           ";AccountKey=" + imgUp.StorageAccountKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account != null) { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                imgUp.Tasks.Add(new Tasks("Azure OCR\tGet container", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (await cbc.ExistsAsync() == false) return new BadRequestObjectResult("Illegal input: Cannot find Container with name: " + imgUp.ContainerName);

                //Write new or append OCR results to separete files
                //string sOCRRes = "All OCR Results.json";
                //CloudBlockBlob jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);

                ////REST interface method, has been failing on v2.0 for due to Newtonsoft 10.0.3 issue
                //ComputerVisionClient cvc = new ComputerVisionClient(
                //    new ApiKeyServiceClientCredentials(Keys.Azure.VisionKey),
                //    new System.Net.Http.DelegatingHandler[] { })
                //{
                //    Endpoint = Keys.Azure.VisionUri
                //};

                //Direct REST method if 'ComputerVisionClient' fails
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", Keys.Azure.VisionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = Keys.Azure.VisionUri + "?" + requestParameters;

                List<Task> lOCRTasks = new List<Task>();

                foreach (ImageGrids ig in imgUp.SplitImages.Where(i => i.ProcessedAzure == false))
                {
                    ////REST interface method, fails due to Newtonsoft 10.0.3 issue
                    //Task<OcrResult> task = Task.Run(async () =>
                    //        res = await cvc.RecognizePrintedTextAsync(false, ig.BlobURL)
                    //    );
                    //task.Wait();

                    lOCRTasks.Add(Task.Run(async () =>
                    {
                        HttpResponseMessage response;

                        OcrResult res = null;

                        //REST direct method
                        Uri uriBlob = new Uri(ig.BlobUrl);
                        CloudBlockBlob blockBlobOrig = new CloudBlockBlob(uriBlob);

                        int iTask = 0;
                        byte[] byteData = new byte[blockBlobOrig.StreamWriteSizeInBytes];

                        iTask = await blockBlobOrig.DownloadToByteArrayAsync(byteData, 0);
                        log.LogInformation(ig.Name + " dowloaded (" + byteData.LongLength.ToString("#,###") + " bytes)");

                        try
                        {
                            // Add the byte array as an octet stream to the request body.
                            using (ByteArrayContent content = new ByteArrayContent(byteData))
                            {
                                // This example uses the "application/octet-stream" content type.
                                // The other content types you can use are "application/json"
                                // and "multipart/form-data".
                                content.Headers.ContentType =
                                    new MediaTypeHeaderValue("application/octet-stream");

                                // Asynchronously call the REST API method.
                                //response = client.PostAsync(uri, content).Result;
                                log.LogInformation(ig.Name + " OCR requested");
                                response = await client.PostAsync(uri, content);
                                log.LogInformation(ig.Name + " OCR responded");
                            }

                            // Asynchronously get the JSON response.
                            string contentString = response.Content.ReadAsStringAsync().Result;
                            res = JsonConvert.DeserializeObject<OcrResult>(contentString);

                            imgUp.Tasks.Add(new Tasks("Azure OCR\tProcessed image " + imgUp.SplitImages.IndexOf(ig).ToString("000"), imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                            log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ": " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());
                            List<DataSummitModels.Cloud.Consolidated.Sentences> sentences = new List<DataSummitModels.Cloud.Consolidated.Sentences>();

                            if (res != null)
                            {
                                Methods.OCRSources ocrMethods = new Methods.OCRSources();
                                BlobOCR o = new BlobOCR();
                                o.Results.Language = res.Language;
                                o.Results.Orientation = res.Orientation;
                                o.Results.TextAngle = (double)res.TextAngle;
                                if (res.Regions != null)
                                {
                                    foreach (OcrRegion r in res.Regions.ToList())
                                    {
                                        o.Results.Regions.Add(Region.CastTo(r));
                                    }
                                }
                                string sOut = String.Join(" ", o.Results.Regions.SelectMany(r => r.Lines.SelectMany(l => l.Words.Select(y => y.Text))));

                                lRes.Add(o);
                                sentences.AddRange(ocrMethods.FromAzure(o.Results));

                                if (ig.Sentences == null) ig.Sentences = new List<DataSummitModels.DB.Sentences>();
                                ig.Sentences.AddRange(Methods.WordLocation.Corrected(sentences, ig));

                                imgUp.Tasks.Add(new Tasks("Azure OCR\tUnified image " + imgUp.SplitImages.IndexOf(ig).ToString("000") + " results", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ": " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());
                            }
                        }
                        catch (OperationCanceledException e)
                        {
                            log.LogInformation(ig.Name + " task cancelled due to '" + e.Message + "'");
                            ig.ProcessedAzure = false;
                        }
                    }));
                }

                imgUp.Tasks.Add(new Tasks("Azure OCR\tAll OCR tasks started", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                List<Task> ltFailed = new List<Task>();
                List<Task> lTPassed = new List<Task>();
                try
                {
                    await Task.WhenAll(lOCRTasks.ToArray());
                }
                catch (Exception)
                {
                    ltFailed = lOCRTasks.Where(t => t.IsCompleted).ToList();
                    lTPassed = lOCRTasks.Where(t => !t.IsCompleted).ToList();
                    lOCRTasks.RemoveAll(t => t.IsCanceled || t.IsFaulted);
                }

                imgUp.Tasks.Add(new Tasks("Azure OCR\tAll OCR tasks finished", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Extract sentences from each ImageGrid and consolidate into ImageUpload (technical duplicate)
                if (imgUp.Sentences == null) imgUp.Sentences = new List<DataSummitModels.DB.Sentences>();
                foreach (ImageGrids ig in imgUp.SplitImages.Where(t => t.ProcessedAzure == false))
                {
                    if (ig.Sentences != null)
                    {
                        imgUp.Sentences.AddRange(ig.Sentences);
                        ig.ProcessedAzure = true;        //Switches state for Azure Function iterations
                        ig.Sentences = null;
                    }
                    ig.IterationAzure = (byte)(ig.IterationAzure + 1);  //Keeps track of number attempts to OCR image
                }

                Self self = new Self();

                //TODO correct method to remove error from this section
                //List<DataSummitModels.Cloud.Consolidated.Sentences> lResults = self.Clean(
                //                                     imgUp.Sentences.Select(s => s.ToModelConsolidated()).ToList())
                //                                    .Select(s => s.ToModel()).ToList();
                //imgUp.Sentences = lResults.Where(s => s.IsUsed == true).ToList();

                imgUp.Tasks.Add(new Tasks("Azure OCR\t'All OCR Results' uploaded", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

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
