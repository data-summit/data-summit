using DataSummitModels.Cloud;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AzureFunctions.MachineLearning
{
    public static class Classification
    {
        private static readonly string ENDPOINT = @"https://documentlayout.cognitiveservices.azure.com";

        [FunctionName("Classification")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject<CustomVision>(jsonContent);
                CustomVision customVisionData = (CustomVision)data;
                
                //Verify body content
                if (string.IsNullOrEmpty(customVisionData.BlobUrl)) return new BadRequestObjectResult("Illegal input: blob url required.");
                if (string.IsNullOrEmpty(customVisionData.MLUrl)) return new BadRequestObjectResult("Illegal input: Ml end-point url required.");
                if (string.IsNullOrEmpty(customVisionData.TrainingKey)) return new BadRequestObjectResult("Illegal input: No training key");
                if (string.IsNullOrEmpty(customVisionData.PredictionKey)) return new BadRequestObjectResult("Illegal input: No prediction key");
                if (string.IsNullOrEmpty(customVisionData.MLProjectName)) return new BadRequestObjectResult("Illegal input: ML project name required");

                // Create a training endpoint, passing in the obtained training key
                var trainingApi = new CustomVisionTrainingClient(
                    new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(customVisionData.TrainingKey))
                { Endpoint = ENDPOINT };

                var projects = await trainingApi.GetProjectsAsync();
                if (projects?.Any() ?? false)
                {
                    var project = projects.Single(p => p.Name == customVisionData.MLProjectName);

                    var preds = new List<ClassificationPrediction>();

                    // Create a prediction endpoint, passing in the obtained prediction key
                    var predictionApi = new CustomVisionPredictionClient(
                        new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(customVisionData.PredictionKey));
                    predictionApi.Endpoint = ENDPOINT;

                    var iteration = customVisionData.TrainingIteration;
                    var imageUrl = new ImageUrl(customVisionData.BlobUrl);
                    var imageClassification = predictionApi.ClassifyImageUrl(project.Id, iteration, imageUrl, customVisionData.BlobUrl);
                    var predictions = imageClassification.Predictions.Where(p => p.Probability > customVisionData.MinThreshold).ToList();

                    foreach (var prediction in imageClassification.Predictions)
                    {
                        if (prediction.Probability > customVisionData.MinThreshold)
                        {
                            var pred = new ClassificationPrediction()
                            {
                                Probability = prediction.Probability,
                                TagId = prediction.TagId,
                                TagName = prediction.TagName,
                                TagType = prediction.TagType
                            };
                            preds.Add(pred);
                        }
                    }
                    var jsonToReturn = JsonConvert.SerializeObject(preds);
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