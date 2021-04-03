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
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ObjectDetectionService(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        public async Task<List<MLPrediction>> GetPrediction(string url, 
                                                            Tuple<string, string> azureFunction, 
                                                            AzureMLResource azureAI, 
                                                            double minThreshold = 0.65)
        {
            var results = new List<MLPrediction>();

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
                results = JsonConvert.DeserializeObject<List<MLPrediction>>(response);
            }
            return results;
        }
    }
}
