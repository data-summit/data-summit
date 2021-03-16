using AzureFunctions.Methods.PostProcessing;
using AzureFunctions.Models.Google.Request;
using AzureFunctions.Models.Google.Response;

using DataSummitModels.DB;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using DataSummitModels.DTO;

namespace AzureFunctions.RecogniseText
{
    public static class Google
    {
        [FunctionName("RecogniseTextGoogle")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;
                name = name ?? data?.name;

                List<FunctionTask> Tasks = new List<FunctionTask>();

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
                //if (Tasks.Count == 0) return new BadRequestObjectResult("Illegal input: 'Tasks' list is empty");

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

                Tasks.Add(new FunctionTask("Google OCR\tGet container", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                log.LogInformation(imgUp.Tasks[Tasks.Count - 1].Name + ":" + imgUp.Tasks[Tasks.Count - 1].Duration.ToString());

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (await cbc.ExistsAsync() == false)
                { return new BadRequestObjectResult("Illegal input: Cannot find Container with name: " + imgUp.ContainerName); }
                else
                {
                    //Write new or append OCR results to separete files
                    //string sOCRRes = "All OCR Results.json";
                    //CloudBlockBlob jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);

                    Cloud gCloud = new Cloud();
                    //Google's literal version:  Uri uri = new Uri("POST https://vision.googleapis.com/v1/images:annotate?key=AIzaSyDMe0LtaxFvFvDCTaEV-05IT792tvxpmbA");
                    Uri uri = new Uri("https://vision.googleapis.com/v1/images:annotate?key=" + "Keys.Google.API_Key");  //Replace with actual key from Azure Secrets

                    List<System.Threading.Tasks.Task> lOCRTasks = new List<System.Threading.Tasks.Task>();

                    foreach (ImageGrids ig in imgUp.SplitImages)
                    {
                        lOCRTasks.Add(System.Threading.Tasks.Task.Run(async () =>
                        {
                            //Download blob image data
                            Uri uriBlob = new Uri(ig.BlobUrl);
                            CloudBlockBlob blockBlobOrig = new CloudBlockBlob(uriBlob);
                            byte[] byteData = new byte[blockBlobOrig.StreamWriteSizeInBytes];
                            await blockBlobOrig.DownloadToByteArrayAsync(byteData, 0);

                            //List<Responses> lrs = new List<Responses>();

                            Requests reqGoogle = new Requests();
                            ImageAndFeat iaf = new ImageAndFeat();
                            iaf.features.Add(new Features("TEXT_DETECTION"));
                            iaf.image.content = Convert.ToBase64String(byteData);
                            reqGoogle.requests.Add(iaf);
                            String json = JsonConvert.SerializeObject(reqGoogle);


                            HttpWebRequest webReqGoogle = (HttpWebRequest)WebRequest.Create(uri);
                            //req1.Timeout = 3600;
                            webReqGoogle.Method = "POST";
                            webReqGoogle.ContentType = "application/json";
                            webReqGoogle.ContentLength = json.Length;

                            Stream stream = webReqGoogle.GetRequestStream();
                            byte[] buffer = Encoding.UTF8.GetBytes(json);
                            stream.Write(buffer, 0, buffer.Length);
                            stream.Close();

                            WebResponse resp = default(WebResponse);

                            try
                            {
                                resp = (HttpWebResponse)webReqGoogle.GetResponse();
                            }
                            catch (WebException wex)
                            {
                                if (wex.Response != null)
                                {
                                    using (var errorResponse = (HttpWebResponse)wex.Response)
                                    {
                                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                        {
                                            string error = reader.ReadToEnd();

                                            //TODO: use JSON.net to parse this string and look at the error message
                                        }
                                    }
                                }
                            }

                            string responseText = String.Empty;
                            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                            {
                                responseText = reader.ReadToEnd();
                            }

                            Cloud res = JsonConvert.DeserializeObject<Cloud>(responseText);
                            List<DataSummitModels.Cloud.Consolidated.Sentences> sentences = new List<DataSummitModels.Cloud.Consolidated.Sentences>();

                            if (res != null)
                            {
                                Methods.OCRSources ocrMethods = new Methods.OCRSources();
                                sentences.AddRange(ocrMethods.FromGoogle(res));

                                if (ig.Sentences == null) ig.Sentences = new List<DataSummitModels.DB.Sentence>();
                                ig.Sentences.AddRange(Methods.WordLocation.Corrected(sentences, ig));

                                Tasks.Add(new FunctionTask("Google OCR\tUnified image " + imgUp.SplitImages.IndexOf(ig).ToString("000") + " results", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                                log.LogInformation(imgUp.Tasks[Tasks.Count - 1].Name + ": " + imgUp.Tasks[Tasks.Count - 1].Duration.ToString());
                            }
                        }));
                    }

                    Tasks.Add(new FunctionTask("Google OCR\tAll OCR tasks started", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                    log.LogInformation(imgUp.Tasks[Tasks.Count - 1].Name + ":" + imgUp.Tasks[Tasks.Count - 1].Duration.ToString());

                    System.Threading.Tasks.Task.WaitAll(lOCRTasks.ToArray());

                    Tasks.Add(new FunctionTask("Google OCR\tAll OCR tasks finished", imgUp.Tasks[Tasks.Count - 1].TimeStamp));
                    log.LogInformation(imgUp.Tasks[Tasks.Count - 1].Name + ":" + imgUp.Tasks[Tasks.Count - 1].Duration.ToString());

                    //Extract sentences from each ImageGrid and consolidate into ImageUpload (technical duplicate)
                    if (imgUp.Sentences == null) imgUp.Sentences = new List<DataSummitModels.DB.Sentence>();
                    foreach (ImageGrids ig in imgUp.SplitImages)
                    {
                        imgUp.Sentences.AddRange(ig.Sentences);
                        ig.Sentences = null;
                    }

                    Self self = new Self();

                    //TODO correct method to remove error from this section
                    //List<DataSummitModels.Cloud.Consolidated.Sentences> lResults = self.Clean(imgUp
                    //                        .Sentences.Select(s => s.ToModelConsolidated()).ToList())
                    //                        .Select(s => s.ToModel()).ToList();
                    //imgUp.Sentences = lResults.Where(s => s.IsUsed == true).ToList();


                    Tasks.Add(new FunctionTask("Google OCR\t'All OCR Results' uploaded", imgUp.Tasks[Tasks.Count - 1].TimeStamp));

                    string jsonToReturn = JsonConvert.SerializeObject(imgUp);

                    return new OkObjectResult(jsonToReturn);
                }
            }
            catch (Exception ae)
            {
                //return error generated within function code
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ae));
            }
        }
    }
}