﻿using DataSummitHelper.Dao.Interfaces;
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
    public class ObjectDetectionService : IObjectDetectionService
    {
        private readonly IDataSummitDao _dao;
        private readonly IDataSummitHelperService _dataSummitHelper;

        public ObjectDetectionService(IDataSummitDao dao, IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _dao = dao ?? throw new ArgumentNullException(nameof(dao)); ;
        }

        public async Task<List<MLPrediction>> GetPrediction(string url, string azureMLResourceName, 
            string azureResourceName, double minThreshold = 0.65)
        {
            var results = new List<MLPrediction>();
            var azureFunction = _dao.GetAzureUrlByName(azureResourceName);
            var azureAI = _dao.GetMLUrlByName(azureMLResourceName);

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
