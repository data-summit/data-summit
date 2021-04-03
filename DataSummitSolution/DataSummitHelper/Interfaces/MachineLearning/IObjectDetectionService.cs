using DataSummitModels.Cloud;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces.MachineLearning
{
    public interface IObjectDetectionService
    {
        Task<List<MLPrediction>> GetPrediction(string url, Tuple<string, string> azureFunction, AzureMLResource azureAI, double minThreshold = 0.65);
    }
}
