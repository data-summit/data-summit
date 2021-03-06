using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using DataSummitModels.Cloud;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace AzureFunctions.Classification
{
    public static class DocumentType
    {
        [FunctionName("DocumentType")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];

            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<CustomVision>(jsonContent);
                CustomVision cvML = (CustomVision)data;

                //Verify body content
                if (cvML.BlobUrl == "") return new BadRequestObjectResult("Illegal input: blob url required.");
                if (cvML.MLUrl == "") return new BadRequestObjectResult("Illegal input: Ml end-point url required.");
                if (cvML.TrainingKey == "") return new BadRequestObjectResult("Illegal input: No training key");
                if (cvML.PredictionKey == "") return new BadRequestObjectResult("Illegal input: No prediction key");
                if (cvML.MLProjectName == "") return new BadRequestObjectResult("Illegal input: ML project name required");

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + cvML.StorageAccountName +
                                           ";AccountKey=" + cvML.StorageKey + ";EndpointSuffix=core.windows.net";

                //CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                //string strError = "Blob connection";
                //if (account == null) { log.LogInformation(strError + ": failed"); }
                //else { log.LogInformation(strError + ": success"); }

                //CloudBlobClient blobClient = account.CreateCloudBlobClient();
                //strError = "CloudBlobClient";
                //if (blobClient.ToString() == "") { log.LogInformation(strError + ": failed"); }
                //else { log.LogInformation(strError + " = " + blobClient.ToString() + ": success"); }

                ////if (cvML.Tasks.Count == 0)
                ////{ cvML.Tasks.Add(new Tasks("Divide Images\tGet container", DateTime.Now)); }
                ////else
                ////{ cvML.Tasks.Add(new Tasks("Divide Images\tGet container", cvML.Tasks[cvML.Tasks.Count - 1].TimeStamp)); }

                ////Get Container name from input object, exit if not found
                //CloudBlobContainer cbc = blobClient.GetContainerReference(cvML.ContainerName);
                //if (await cbc.ExistsAsync() == false)
                //{ return new BadRequestObjectResult("Illegal input: Cannot find Container with name: " + cvML.ContainerName); }

                ////Find 'Original.jpg' blob
                //CloudBlockBlob cbbOrig = cbc.GetBlockBlobReference("Original.jpg");

                //////As stream
                ////MemoryStream ms = new MemoryStream();
                ////await cbbOrig.DownloadToStreamAsync(ms);
                ////ms.Seek(0, SeekOrigin.Begin);

                //////As byte array
                ////byte[] ba = new byte[cbbOrig.Properties.Length];
                ////await cbbOrig.DownloadToByteArrayAsync(ba, 0);

                // Create a training endpoint, passing in the obtained training key
                CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(
                    new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(cvML.TrainingKey))
                { Endpoint = "https://documentlayout.cognitiveservices.azure.com/" };

                var projects = await trainingApi.GetProjectsAsync();
                if (projects != null && projects.Count(p => p.Name == cvML.MLProjectName) > 0)
                {
                    var project = projects.First(p => p.Name == cvML.MLProjectName);

                    string resName = "";

                    // Create a prediction endpoint, passing in the obtained prediction key
                    CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(
                        new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(cvML.PredictionKey));
                    predictionApi.Endpoint = "https://documentlayout.cognitiveservices.azure.com/"; // cvML.MLUrl;
                    //predictionApi.HttpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    //var result = predictionApi.ClassifyImage(project.Id, cvML.MLProjectName, ms);
                    var result = predictionApi.ClassifyImageUrl(project.Id, "Iteration4", new ImageUrl(cvML.BlobUrl) { Url = cvML.BlobUrl });
                    
                    foreach (var c in result.Predictions)
                    { Console.WriteLine($"\t{c.TagName}: {c.Probability:P1}"); }

                    resName = result.Predictions.Where(p => p.Probability == result.Predictions.Max(q => q.Probability)).First().TagName;

                    string jsonToReturn = JsonConvert.SerializeObject(resName);
                    return new OkObjectResult(jsonToReturn);
                }
                else
                {
                    return new BadRequestObjectResult(JsonConvert.SerializeObject("Project cannot be found"));
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