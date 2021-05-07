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
        private readonly IAzureResourcesService _azureResources;
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IDataSummitDocumentsDao _dataSummitDocumentsDao;
        private readonly IDataSummitDocumentsService _dataSummitDocumentsService;

        public ObjectDetectionService(IDataSummitAzureUrlsDao azureDao,
                                     IAzureResourcesService azureResources,
                                     IDataSummitHelperService dataSummitHelper,
                                     IDataSummitDocumentsDao dataSummitDocumentsDao,
                                     IDataSummitDocumentsService dataSummitDocumentsService)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _azureDao = azureDao ?? throw new ArgumentNullException(nameof(azureDao));
            _azureResources = azureResources ?? throw new ArgumentNullException(nameof(azureResources));
            _dataSummitDocumentsDao = dataSummitDocumentsDao ?? throw new ArgumentNullException(nameof(dataSummitDocumentsDao));
            _dataSummitDocumentsService = dataSummitDocumentsService ?? throw new ArgumentNullException(nameof(dataSummitDocumentsService));
        }

        public async Task<KeyValuePair<string, int>> GetDrawingLayout(string url)
        {
            List<DocumentFeature> Features = new List<DocumentFeature>();

            int predictionAccuracy = 3;
            int boundingBoxAccuracy = 5;

            var drawingLayout = await GetPrediction(url, "DrawingLayout", "ObjectDetection");
            if (drawingLayout != null && drawingLayout.Count > 0)
            {
                var doc = _dataSummitDocumentsDao.GetDocumentByUrl(url);
                foreach (var item in drawingLayout)
                {
                    var itemType = _dataSummitDocumentsService.DrawingLayoutComponent(item.TagName);
                    var typeConfidence = Math.Round(item.Probability, predictionAccuracy);

                    var docFeature = new DocumentFeature()
                    {
                        Confidence = (decimal)typeConfidence,
                        Feature = "Object",
                        Value = itemType.ToString(),
                        Height = (long)Math.Abs(item.BoundingBox.Max.X - item.BoundingBox.Min.X),
                        Width = (long)Math.Abs(item.BoundingBox.Max.Y - item.BoundingBox.Min.Y),
                        Vendor = "Custom Vision"
                    };

                    // Top
                    if (item.BoundingBox.Min.Y < item.BoundingBox.Max.Y)
                    { docFeature.Top = (decimal)Math.Round(item.BoundingBox.Max.Y, boundingBoxAccuracy); }
                    else
                    { docFeature.Top = (decimal)Math.Round(item.BoundingBox.Min.Y, boundingBoxAccuracy); }
                    // Left
                    if (item.BoundingBox.Min.X > item.BoundingBox.Max.X)
                    { docFeature.Left = (decimal)Math.Round(item.BoundingBox.Max.X, boundingBoxAccuracy); }
                    else
                    { docFeature.Left = (decimal)Math.Round(item.BoundingBox.Min.X, boundingBoxAccuracy); }
                    Features.Add(docFeature);                   
                }
                // Remove existing features
                doc.DocumentFeatures.Clear();
                // Add new detected features
                doc.DocumentFeatures = Features;

                _dataSummitDocumentsDao.UpdateDocument(doc);
            }
            return new KeyValuePair<string, int>(url, Features.Count);
        }

        public async Task<List<MLPrediction>> GetPrediction(string url, string azureMLResourceName,
            string azureResourceName, double minThreshold = 0.15)
        {
            var result = new List<MLPrediction>();
            var azureFunction = await _azureDao.GetAzureFunctionUrlByName(azureResourceName);
            var azureAI = await _azureDao.GetMLUrlByNameAsync(azureMLResourceName);

            if (azureFunction != null && azureAI != null)
            {
                var customVisionRequest = new CustomVision()
                {
                    BlobUrl = url,
                    MLUrl = azureAI.Url,    
                    TrainingKey = azureAI.TrainingKey,
                    PredictionKey = azureAI.PredicitionKey,
                    MLProjectName = azureAI.Name,
                    MinThreshold = minThreshold
                };

                //TODO catch error responses
                var httpResponse = await _dataSummitHelper.ProcessCall(new Uri(azureFunction.Item1 + "?code=" + azureFunction.Item2),
                    JsonConvert.SerializeObject(customVisionRequest));
                var response = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<List<MLPrediction>>(response);
                result = results.OrderBy(f => f.Probability).ToList();
            }
            return result;
        }
    }
}
