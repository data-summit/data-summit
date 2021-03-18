using DataSummitModels.Cloud;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces.MachineLearning
{
    public interface IClassificationService
    {
        Task<List<MLPrediction>> DocumentType(string url, string azureMLResourceName, string azureResourceName);
    }
}
