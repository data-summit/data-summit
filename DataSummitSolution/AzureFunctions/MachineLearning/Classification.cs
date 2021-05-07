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
        private static readonly string endPoint = @"https://documentlayout.cognitiveservices.azure.com";
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
                if (customVisionData.BlobUrl == "") return new BadRequestObjectResult("Illegal input: blob url required.");
                if (customVisionData.MLUrl == "") return new BadRequestObjectResult("Illegal input: Ml end-point url required.");
                if (customVisionData.TrainingKey == "") return new BadRequestObjectResult("Illegal input: No training key");
                if (customVisionData.PredictionKey == "") return new BadRequestObjectResult("Illegal input: No prediction key");
                if (customVisionData.MLProjectName == "") return new BadRequestObjectResult("Illegal input: ML project name required");

                // Create a training endpoint, passing in the obtained training key
                CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(
                    new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(customVisionData.TrainingKey))
                { Endpoint = endPoint };

                var projects = await trainingApi.GetProjectsAsync();
                if (projects != null && projects.Count(p => p.Name == customVisionData.MLProjectName) > 0)
                {
                    var project = projects.First(p => p.Name == customVisionData.MLProjectName);

                    List<MLPrediction> preds = new List<MLPrediction>();
                    if (project != null)
                    {
                        // Create a prediction endpoint, passing in the obtained prediction key
                        CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(
                        new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(customVisionData.PredictionKey));
                        predictionApi.Endpoint = endPoint;

                        var iteration = customVisionData.GetIteration();
                        var result = predictionApi.ClassifyImageUrl(project.Id, iteration, new ImageUrl(customVisionData.BlobUrl) { Url = customVisionData.BlobUrl });

                        foreach (var c in result.Predictions)
                        {
                            if (c.Probability > customVisionData.MinThreshold)
                            {
                                var pred = new MLPrediction()
                                {
                                    Probability = c.Probability,
                                    TagId = c.TagId,
                                    TagName = c.TagName,
                                    TagType = c.TagType
                                };
                                preds.Add(pred);
                            }
                        }
                    }
                    string jsonToReturn = JsonConvert.SerializeObject(preds);
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