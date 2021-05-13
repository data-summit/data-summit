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
    public class ClassificationService : IClassificationService
    {
        private readonly IDataSummitAzureUrlsDao _azureDao;
        private readonly IAzureResourcesService _azureResources;
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IDataSummitDocumentsDao _dataSummitDocumentsDao;
        private readonly IDataSummitDocumentsService _dataSummitDocumentsService;

        public ClassificationService(IDataSummitAzureUrlsDao azureDao,
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

        public async Task<KeyValuePair<string, string>> GetDocumentType(string url)
        {
            var documentTypeClassification = await GetClassificationPrediction(url, "DocumentType", "Classification");
            var documentTypeEnum = _dataSummitDocumentsService.DocumentType(documentTypeClassification.TagName);
            
            var typeConfidence = Math.Round(documentTypeClassification.Probability, 3);

            // Update blob metadata
            List<KeyValuePair<string, string>> additionalMetaData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("DocumentType", documentTypeEnum.ToString()),
                        new KeyValuePair<string, string>("DocumentTypeConfidence", typeConfidence.ToString())
                    };

            await _azureResources.AddMetadataToBlob(url, additionalMetaData);

            //Persist in database
            var doc = _dataSummitDocumentsDao.GetDocumentByUrl(url);
            doc.DocumentType = new DocumentType()
            {
                Name = documentTypeEnum.ToString(),
                DocumentTypeId = (byte)documentTypeEnum
            };

            doc.AzureConfidence = (decimal)typeConfidence;
            _dataSummitDocumentsDao.UpdateDocument(doc);

            return new KeyValuePair<string, string>(url, documentTypeEnum.ToString());
        }

        public async Task<ClassificationPrediction> GetClassificationPrediction(string url, string azureMLResourceName, 
            string azureResourceName, double minThreshold = 0.65)
        {
            var result = new ClassificationPrediction();
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
                
                var results = JsonConvert.DeserializeObject<List<ClassificationPrediction>>(response);
                result = results.OrderBy(f => f.Probability).First();
            }

            return result;
        }
    }
}
