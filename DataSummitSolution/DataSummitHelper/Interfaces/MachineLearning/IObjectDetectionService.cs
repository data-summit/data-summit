using DataSummitModels.Cloud;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces.MachineLearning
{
    public interface IObjectDetectionService
    {
        Task<List<MLPrediction>> GetPrediction(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.65);
    }
}
