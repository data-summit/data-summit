using DataSummitService.Dao.Interfaces;
using DataSummitService.Interfaces;
using DataSummitService.Interfaces.MachineLearning;
using DataSummitModels.Cloud;
using DataSummitModels.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitService.Services.MachineLearning
{
    public class ObjectDetectionService : IObjectDetectionService
    {
        private readonly IDataSummitAzureUrlsDao _azureDao;
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IDataSummitDocumentsDao _dataSummitDocumentsDao;
        private readonly IDataSummitDocumentsService _dataSummitDocumentsService;

        public ObjectDetectionService(IDataSummitAzureUrlsDao azureDao,
                                     IDataSummitHelperService dataSummitHelper,
                                     IDataSummitDocumentsDao dataSummitDocumentsDao,
                                     IDataSummitDocumentsService dataSummitDocumentsService)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _azureDao = azureDao ?? throw new ArgumentNullException(nameof(azureDao));
            _dataSummitDocumentsDao = dataSummitDocumentsDao ?? throw new ArgumentNullException(nameof(dataSummitDocumentsDao));
            _dataSummitDocumentsService = dataSummitDocumentsService ?? throw new ArgumentNullException(nameof(dataSummitDocumentsService));
        }

        public async Task<KeyValuePair<string, int>> GetDrawingLayout(string url)
        {
            var features = new List<DocumentFeature>();

            int predictionAccuracy = 3;
            int boundingBoxAccuracy = 5;

            var drawingLayoutComponents = await GetObjectDetectionPredictions(url, "DrawingLayout", "ObjectDetection");
            if (drawingLayoutComponents?.Any() ?? false)
            {
                var doc = _dataSummitDocumentsDao.GetDocumentByUrl(url);
                foreach (var drawingLayoutComponent in drawingLayoutComponents)
                {
                    var drawingLayounEnum = _dataSummitDocumentsService.DrawingLayoutComponent(drawingLayoutComponent.TagName);
                    var typeConfidence = Math.Round(drawingLayoutComponent.Probability, predictionAccuracy);

                    var docFeature = new DocumentFeature()
                    {
                        Confidence = (decimal)typeConfidence,
                        Feature = "Object",
                        Value = drawingLayounEnum.ToString(),
                        Height = (long)Math.Abs(drawingLayoutComponent.BoundingBox.Max.X - drawingLayoutComponent.BoundingBox.Min.X),
                        Width = (long)Math.Abs(drawingLayoutComponent.BoundingBox.Max.Y - drawingLayoutComponent.BoundingBox.Min.Y),
                        Vendor = "Custom Vision"
                    };

                    // Top
                    if (drawingLayoutComponent.BoundingBox.Min.Y < drawingLayoutComponent.BoundingBox.Max.Y)
                    { docFeature.Top = (decimal)Math.Round(drawingLayoutComponent.BoundingBox.Max.Y, boundingBoxAccuracy); }
                    else
                    { docFeature.Top = (decimal)Math.Round(drawingLayoutComponent.BoundingBox.Min.Y, boundingBoxAccuracy); }
                    // Left
                    if (drawingLayoutComponent.BoundingBox.Min.X > drawingLayoutComponent.BoundingBox.Max.X)
                    { docFeature.Left = (decimal)Math.Round(drawingLayoutComponent.BoundingBox.Max.X, boundingBoxAccuracy); }
                    else
                    { docFeature.Left = (decimal)Math.Round(drawingLayoutComponent.BoundingBox.Min.X, boundingBoxAccuracy); }
                    features.Add(docFeature);                   
                }
                // Remove existing features
                doc.DocumentFeatures.Clear();
                // Add new detected features
                doc.DocumentFeatures = features;

                _dataSummitDocumentsDao.UpdateDocument(doc);
            }
            return new KeyValuePair<string, int>(url, features.Count);
        }

        public async Task<List<ObjectDetectionPrediction>> GetObjectDetectionPredictions(string url, string azureMLResourceName,
            string azureResourceName, double minThreshold = 0.15)
        {
            var result = new List<ObjectDetectionPrediction>();
            var azureFunction = await _azureDao.GetAzureFunctionUrlByName(azureResourceName);
            var azureML = await _azureDao.GetAzureMLResourceByNameAsync(azureMLResourceName);

            if (azureFunction != null && azureML != null)
            {
                var customVisionRequest = new CustomVision()
                {
                    BlobUrl = url,
                    MLUrl = azureML.Url,    
                    TrainingKey = azureML.TrainingKey,
                    PredictionKey = azureML.PredicitionKey,
                    MLProjectName = azureML.Name,
                    MinThreshold = minThreshold
                };

                //TODO catch error responses
                var httpResponse = await _dataSummitHelper.ProcessCall(new Uri(azureFunction.Item1 + "?code=" + azureFunction.Item2),
                    JsonConvert.SerializeObject(customVisionRequest));
                var response = await httpResponse.Content.ReadAsStringAsync();

                var objectDetectionPredictions = JsonConvert.DeserializeObject<List<ObjectDetectionPrediction>>(response);
                result = objectDetectionPredictions.OrderBy(f => f.Probability).ToList();
            }
            return result;
        }
    }
}
