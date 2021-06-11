using DataSummitModels.Cloud;
using DataSummitService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces.MachineLearning
{
    public interface IClassificationService
    {
        Task<DocumentTypeSummaryDto> GetDocumentType(string url);
        Task<ClassificationPrediction> GetClassificationPrediction(string url, string azureMLResourceName, string azureResourceName, double minThreshold = 0.65);
    }
}
