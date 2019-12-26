#region References
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using AWS = Amazon;

using DataSummitFunctions.Models;
using DataSummitFunctions.Models.Amazon;
using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Methods.PostProcessing;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
#endregion

namespace DataSummitFunctions
{
    public static class RecogniseTextAmazon
    {
        [FunctionName("RecogniseTextAmazon")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                 HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<ImageUpload>(jsonContent);
                ImageUpload imgUp = (ImageUpload)data;

                //Validate entry data
                if (imgUp.FileName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: File name is ,less than zero.");
                if (imgUp.Type == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Type is blank.");
                if (imgUp.BlobStorageName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage name required.");
                if (imgUp.BlobStorageKey == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Storage key required.");
                if (imgUp.WidthOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Image must have width greater than zero");
                if (imgUp.HeightOriginal <= 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Image must have height greater than zero");
                if (imgUp.ContainerName == "") return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Container must have a GUID name");
                if (imgUp.SplitImages == null) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: 'SplitImages' list is null");
                if (imgUp.SplitImages.Count == 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: 'SplitImages' list is empty");
                if (imgUp.Tasks == null) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: 'Tasks' list is null");
                if (imgUp.Tasks.Count == 0) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: 'Tasks' list is empty");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + imgUp.BlobStorageName +
                                           ";AccountKey=" + imgUp.BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                string strError = "Blob connection";
                if (account == null) { log.Info(strError + ": failed"); }
                else { log.Info(strError + ": success"); }

                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                strError = "CloudBlobClient";
                if (blobClient.ToString() == "") { log.Info(strError + ": failed"); }
                else { log.Info(strError + " = " + blobClient.ToString() + ": success"); }

                imgUp.Tasks.Add(new Tasks("Amazon OCR\tGet container", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Get Container name from input object, exit if not found
                CloudBlobContainer cbc = blobClient.GetContainerReference(imgUp.ContainerName);
                if (cbc.Exists() == false) return req.CreateResponse(HttpStatusCode.BadRequest, "Illegal input: Cannot find Container with name: " + imgUp.ContainerName);

                //Write new or append OCR results to separete files
                //string sOCRRes = "All OCR Results.json";
                //CloudBlockBlob jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);

                AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Keys.Amazon.AccessKeyID, Keys.Amazon.SecretAccessKey, AWS.RegionEndpoint.EUWest1);
                DetectTextRequest detectTextRequest = new DetectTextRequest();

                List<Task> lOCRTasks = new List<Task>();

                foreach (ImageGrid ig in imgUp.SplitImages)
                {
                    //lOCRTasks.Add(Task.Run(() =>
                    //{
                    //Download blob image data
                    Uri uriBlob = new Uri(ig.BlobURL);
                    CloudBlockBlob blockBlobOrig = new CloudBlockBlob(uriBlob);
                    MemoryStream ms = new MemoryStream();
                    blockBlobOrig.DownloadToStream(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    detectTextRequest.Image = new AWS.Rekognition.Model.Image();
                    detectTextRequest.Image.Bytes = ms;
                    DetectTextResponse res = rekognitionClient.DetectText(detectTextRequest);
                    List<Models.Consolidated.Sentences> sentences = new List<Models.Consolidated.Sentences>();

                    if (res != null)
                    {
                        Methods.OCRSources ocrMethods = new Methods.OCRSources();
                        sentences.AddRange(ocrMethods.FromAmazon(res.TextDetections, ig));

                        if (ig.Sentences == null) ig.Sentences = new List<Models.Consolidated.Sentences>();
                        ig.Sentences.AddRange(Methods.WordLocation.Corrected(sentences, ig));

                        imgUp.Tasks.Add(new Tasks("Amazon OCR\tUnified image " + imgUp.SplitImages.IndexOf(ig).ToString("000") + " results", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                        log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ": " + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());
                    }
                    //}));
                }

                imgUp.Tasks.Add(new Tasks("Amazon OCR\tAll OCR tasks started", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Task.WaitAll(lOCRTasks.ToArray());

                imgUp.Tasks.Add(new Tasks("Amazon OCR\tAll OCR tasks finished", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));
                log.Info(imgUp.Tasks[imgUp.Tasks.Count - 1].Name + ":" + imgUp.Tasks[imgUp.Tasks.Count - 1].Duration.ToString());

                //Extract sentences from each ImageGrid and consolidate into ImageUpload (technical duplicate)
                if (imgUp.Sentences == null) imgUp.Sentences = new List<Sentences>();
                foreach (ImageGrid ig in imgUp.SplitImages)
                {
                    imgUp.Sentences.AddRange(ig.Sentences);
                    ig.Sentences = null;
                }

                Self self = new Self();
                List<Sentences> lResults = self.Clean(imgUp.Sentences);
                imgUp.Sentences = lResults.Where(s => s.IsUsed == true).ToList();

                //if (jsonSentencesBlob.Exists() == true)
                //{
                //    imgUp.Sentence.AddRange(JsonConvert.DeserializeObject<List<Sentence>>(jsonSentencesBlob.DownloadText()));
                //    jsonSentencesBlob.Delete();
                //}
                //jsonSentencesBlob = cbc.GetBlockBlobReference(sOCRRes);
                //jsonSentencesBlob.UploadText(JsonConvert.SerializeObject(lResults));

                imgUp.Tasks.Add(new Tasks("Amazon OCR\t'All OCR Results' uploaded", imgUp.Tasks[imgUp.Tasks.Count - 1].TimeStamp));

                string jsonToReturn = JsonConvert.SerializeObject(imgUp);

                return new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json") };
            }
            catch (Exception ae)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                { Content = new StringContent(JsonConvert.SerializeObject(ae), Encoding.UTF8, "application/json") };
            }
        }
    }
}
