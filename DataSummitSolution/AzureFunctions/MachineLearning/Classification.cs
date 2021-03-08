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
        [FunctionName("Classification")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
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

                // Create a training endpoint, passing in the obtained training key
                CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(
                    new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(cvML.TrainingKey))
                { Endpoint = "https://documentlayout.cognitiveservices.azure.com/" };

                var projects = await trainingApi.GetProjectsAsync();
                if (projects != null && projects.Count(p => p.Name == cvML.MLProjectName) > 0)
                {
                    var project = projects.First(p => p.Name == cvML.MLProjectName);

                    List<MLPrediction> preds = new List<MLPrediction>();
                    if (project != null)
                    {
                        // Create a prediction endpoint, passing in the obtained prediction key
                        CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(
                        new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(cvML.PredictionKey));
                        predictionApi.Endpoint = "https://documentlayout.cognitiveservices.azure.com/"; // cvML.MLUrl;

                        var result = predictionApi.ClassifyImageUrl(project.Id, "Iteration4", new ImageUrl(cvML.BlobUrl) { Url = cvML.BlobUrl });

                        foreach (var c in result.Predictions)
                        {
                            if (c.Probability > cvML.MinThreshold)
                            {
                                var pred = new MLPrediction()
                                {
                                    Probability = c.Probability,
                                    TagId = c.TagId,
                                    TagName = c.TagName,
                                    TagType = c.TagType
                                };
                                if (c.BoundingBox != null)
                                {
                                    pred.BoundingBox = new DataSummitModels.BHoM.Geometry.BoundingBox()
                                    {
                                        Max = new DataSummitModels.BHoM.Geometry.Point()
                                        {
                                            X = c.BoundingBox.Left + c.BoundingBox.Width,
                                            Y = c.BoundingBox.Top
                                        },
                                        Min = new DataSummitModels.BHoM.Geometry.Point()
                                        {
                                            X = c.BoundingBox.Left,
                                            Y = c.BoundingBox.Top - c.BoundingBox.Height
                                        }
                                    };
                                }
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