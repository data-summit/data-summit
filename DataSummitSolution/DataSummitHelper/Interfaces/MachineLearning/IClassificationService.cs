using DataSummitModels.Cloud;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces.MachineLearning
{
    public interface IClassificationService
    {
        Task<MLPrediction> GetPrediction(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.65);
    }
}
