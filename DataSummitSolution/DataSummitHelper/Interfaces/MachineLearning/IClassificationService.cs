using DataSummitModels.Cloud;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces.MachineLearning
{
    public interface IClassificationService
    {
        Task<KeyValuePair<string, string>> GetDocumentType(string url);
        Task<MLPrediction> GetPrediction(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.65);
    }
}
