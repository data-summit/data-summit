using DataSummitModels.Cloud;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces.MachineLearning
{
    public interface IObjectDetectionService
    {
        Task<KeyValuePair<string, int>> GetDrawingLayout(string url);
        Task<List<ObjectDetectionPrediction>> GetObjectDetectionPredictions(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.15);
    }
}
