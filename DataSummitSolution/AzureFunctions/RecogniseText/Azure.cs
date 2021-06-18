using AzureFunctions.Methods;
using AzureFunctions.Models.Azure;
using DataSummitModels.DB;
using DataSummitModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureFunctions.RecogniseText
{
    public static class Azure
    {
        [FunctionName("RecogniseTextAzure")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = request.Query["name"];

            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                var imageUpload = JsonConvert.DeserializeObject<ImageUpload>(requestBody,
                        new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTime });

                //Validate entry data
                ValidateRequest(imageUpload);

                var blobOCRs = new List<BlobOCR>();
                var document = new Document();

                string connectionString = $"DefaultEndpointsProtocol=https;AccountName={imageUpload.StorageAccountName};AccountKey={imageUpload.StorageAccountKey};EndpointSuffix=core.windows.net";
                var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = cloudStorageAccount.CreateCloudBlobClient();

                var tasksIndex = 0;
                var functionTasks = new List<FunctionTaskDto>
                {
                    new FunctionTaskDto("Azure OCR\tGet container", imageUpload.Tasks[tasksIndex].TimeStamp)
                };
                
                //Get Container name from input object, exit if not found
                var cloudBlobContainer = blobClient.GetContainerReference(imageUpload.ContainerName);
                if (!await cloudBlobContainer.ExistsAsync()) 
                { 
                    return new BadRequestObjectResult($"Illegal input: Cannot find Container with name: {imageUpload.ContainerName}"); 
                }

                var httpClient = new HttpClient();
                //TODO: Replace with actual key from Azure Secrets
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "Keys.Azure.VisionKey");  

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                var ocrTasks = new List<Task>();
                foreach (ImageGrids imageGrids in imageUpload.SplitImages.Where(i => i.ProcessedAzure == false))
                {
                    ocrTasks.Add(Task.Run(async () =>
                    RunOCRTask()));
                }

                functionTasks.Add(new FunctionTaskDto("Azure OCR\tAll OCR tasks started", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));
                
                List<Task> ltFailed = new List<Task>();
                List<Task> lTPassed = new List<Task>();
                try
                {
                    await Task.WhenAll(ocrTasks.ToArray());
                }
                catch (Exception)
                {
                    ltFailed = ocrTasks.Where(t => t.IsCompleted).ToList();
                    lTPassed = ocrTasks.Where(t => !t.IsCompleted).ToList();
                    ocrTasks.RemoveAll(t => t.IsCanceled || t.IsFaulted);
                }

                functionTasks.Add(new FunctionTaskDto("Azure OCR\tAll OCR tasks finished", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                //Extract sentences from each ImageGrid and consolidate into ImageUpload (technical duplicate)
                if (imageUpload.Sentences == null) { imageUpload.Sentences = new List<Sentence>(); }
                foreach (ImageGrids imageGrids in imageUpload.SplitImages.Where(t => t.ProcessedAzure == false))
                {
                    if (imageGrids.Sentences != null)
                    {
                        imageUpload.Sentences.AddRange(imageGrids.Sentences);
                        imageGrids.ProcessedAzure = true; 
                        imageGrids.Sentences = null;
                    }
                    
                    //Keeps track of number attempts to OCR image
                    imageGrids.IterationAzure = imageGrids.IterationAzure++;  
                }
                
                functionTasks.Add(new FunctionTaskDto("Azure OCR\t'All OCR Results' uploaded", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                var processingResult = JsonConvert.SerializeObject(imageUpload);
                return new OkObjectResult(processingResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ex));
            }
        }

        private static async Task RunOCRTask(ImageGrids imageGrids, HttpClient httpClient, List<FunctionTaskDto> functionTasks, ImageUpload imageUpload, List<BlobOCR> blobOCRs)
        {
            HttpResponseMessage response;

            //REST direct method
            var blobUri = new Uri(imageGrids.BlobUrl);
            var blockBlobOrig = new CloudBlockBlob(blobUri);
            var byteData = new byte[blockBlobOrig.StreamWriteSizeInBytes];
            var iTask = await blockBlobOrig.DownloadToByteArrayAsync(byteData, 0);
            
            try
            {
                // Add the byte array as an octet stream to the request body.
                using (var content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    var uri = "Keys.Azure.VisionUri?language=unk&detectOrientation=true";
                    response = await httpClient.PostAsync(uri, content);
                }
                
                functionTasks.Add(new FunctionTaskDto($"Azure OCR\tProcessed image {imageUpload.SplitImages.IndexOf(imageGrids):000}", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                // Asynchronously get the JSON response.
                var sentences = new List<Sentences>();
                string responseContent = response.Content.ReadAsStringAsync().Result;
                var ocrResult = JsonConvert.DeserializeObject<OcrResult>(responseContent);
                if (ocrResult != null)
                {
                    var ocrMethods = new OCRSources();
                    var blobOCR = new BlobOCR();
                    blobOCR.Results.Language = ocrResult.Language;
                    blobOCR.Results.Orientation = ocrResult.Orientation;
                    blobOCR.Results.TextAngle = (double)ocrResult.TextAngle;
                    if (ocrResult.Regions != null)
                    {
                        foreach (var ocrRegion in ocrResult.Regions)
                        {
                            blobOCR.Results.Regions.Add(Region.CastTo(ocrRegion));
                        }
                    }
                    
                    blobOCRs.Add(blobOCR);
                    sentences.AddRange(ocrMethods.FromAzure(blobOCR.Results));

                    if (imageGrids.Sentences == null) imageGrids.Sentences = new List<Sentence>();
                    imageGrids.Sentences.AddRange(WordLocation.Corrected(sentences, imageGrids));

                    functionTasks.Add(new FunctionTaskDto($"Azure OCR\tUnified image {imageUpload.SplitImages.IndexOf(imageGrids):000} results", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));
                }
            }
            catch (OperationCanceledException e)
            {
                log.LogInformation($"{imageGrids.Name} task cancelled due to ' {e.Message} '");
                imageGrids.ProcessedAzure = false;
            }
        }

        private static BadRequestObjectResult ValidateRequest(ImageUpload imageUpload)
        {
            if (string.IsNullOrEmpty(imageUpload.FileName)) { return CreateBadRequestObject(nameof(imageUpload.FileName)); }
            if (string.IsNullOrEmpty(imageUpload.StorageAccountName)) { return CreateBadRequestObject(nameof(imageUpload.StorageAccountName)); }
            if (string.IsNullOrEmpty(imageUpload.StorageAccountKey)) { return CreateBadRequestObject(nameof(imageUpload.StorageAccountKey)); }
            if (string.IsNullOrEmpty(imageUpload.ContainerName)) { return CreateBadRequestObject(nameof(imageUpload.ContainerName)); }
            if (imageUpload.SplitImages?.Any() ?? true) { return CreateBadRequestObject(nameof(imageUpload.SplitImages)); }
            if (imageUpload.Tasks?.Any() ?? true) { return CreateBadRequestObject(nameof(imageUpload.Tasks)); }
            if (imageUpload.WidthOriginal <= 0) { return new BadRequestObjectResult("Illegal input: Image must have width greater than zero"); }
            if (imageUpload.HeightOriginal <= 0) { new BadRequestObjectResult("Illegal input: Image must have height greater than zero"); }

            return null;
        }

        private static BadRequestObjectResult CreateBadRequestObject(string parameterName) 
            => new BadRequestObjectResult($"Illegal input: {parameterName} is null or empty.");
    }
}
