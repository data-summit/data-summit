using DataSummitModels.Cloud;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces.MachineLearning
{
    public interface IObjectDetectionService
    {
        Task<KeyValuePair<string, string>> GetDrawingLayout(string url);
        Task<List<MLPrediction>> GetPrediction(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.15);
    }
}
