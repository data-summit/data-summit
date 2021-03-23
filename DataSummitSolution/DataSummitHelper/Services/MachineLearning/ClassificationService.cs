using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.Cloud;
using DataSummitModels.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper.Services.MachineLearning
{
    public class ClassificationService : IClassificationService
    {
        private readonly IDataSummitDao _dao;
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ClassificationService(IDataSummitDao dao, IDataSummitHelperService dataSummitHelper, DataSummitDbContext dataSummitDbContext)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _dao = dao ?? throw new ArgumentNullException(nameof(dao)); ;
        }

        public async Task<MLPrediction> DocumentType(string url, string azureMLResourceName, 
            string azureResourceName)
        {
            var result = new MLPrediction();
            var azureFunction = await _dao.GetAzureUrlByName(azureResourceName);
            var azureAI = await _dao.GetMLUrlByName(azureMLResourceName);

            if (azureFunction != null && azureAI != null)
            {
                var customVisionRequest = new CustomVision()
                {
                    BlobUrl = url,
                    MLUrl = azureAI.Url,
                    TrainingKey = azureAI.TrainingKey,
                    PredictionKey = azureAI.PredicitionKey,
                    MLProjectName = azureAI.Name,
                    MinThreshold = 0.65
                };

                //TODO catch error responses
                var httpResponse = await _dataSummitHelper.ProcessCall(new Uri(azureFunction.Url + "?code=" + azureFunction.Key),
                    JsonConvert.SerializeObject(customVisionRequest));
                var response = await httpResponse.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<List<MLPrediction>>(response);
                result = results.OrderBy(f => f.Probability).First();

            }
            return result;
        }
    }
}
