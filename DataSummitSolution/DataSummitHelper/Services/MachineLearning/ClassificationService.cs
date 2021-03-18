using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.Cloud;
using DataSummitModels.DB;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper.Services.MachineLearning
{
    public class ClassificationService : IClassificationService
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        private DataSummitDbContext _dbContext;

        public ClassificationService(IDataSummitHelperService dataSummitHelper, DataSummitDbContext dataSummitDbContext)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _dbContext = dataSummitDbContext;
        }

        public async Task<List<MLPrediction>> DocumentType(string url, string azureMLResourceName, 
            string azureResourceName)
        {
            var results = new List<MLPrediction>();
            var azureFunction = _dbContext.AzureCompanyResourceUrls.First(f => f.Name == azureResourceName);
            var azureAI = _dbContext.AzureMLResources.First(a => a.Name == azureMLResourceName);

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
                var httpResponse = await _dataSummitHelper.ProcessCall(new Uri(azureFunction.Url), JsonConvert.SerializeObject(customVisionRequest));
                var response = await httpResponse.Content.ReadAsStringAsync();
                results = JsonConvert.DeserializeObject<List<MLPrediction>>(response);
            }
            return results;
        }
    }
}
