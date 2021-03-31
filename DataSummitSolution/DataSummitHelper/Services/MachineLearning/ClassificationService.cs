using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.Cloud;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper.Services.MachineLearning
{
    public class ClassificationService : IClassificationService
    {
        private readonly IDataSummitAzureUrlsDao _azureDao;
        private readonly IDataSummitMachineLearningDao _machineLearningDao;
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ClassificationService(IDataSummitAzureUrlsDao azureDao,
                                     IDataSummitMachineLearningDao machineLearningDao,
                                     IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _azureDao = azureDao ?? throw new ArgumentNullException(nameof(azureDao)); ;
            _machineLearningDao = machineLearningDao ?? throw new ArgumentNullException(nameof(machineLearningDao)); ;
        }

        public async Task<MLPrediction> GetPrediction(string url, string azureMLResourceName, 
            string azureResourceName, double minThreshold = 0.65)
        {
            var result = new MLPrediction();
            var azureFunction = _azureDao.GetAzureUrlByName(azureResourceName);
            var azureAI = _machineLearningDao.GetMLUrlByName(azureMLResourceName);

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
                result = results.OrderBy(f => f.Probability).First();
            }

            return result;
        }
    }
}
