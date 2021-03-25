using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitDocumentsService
    {
        DataSummitModels.Enums.Document.Type DocumentType(string mimeType);
        DataSummitModels.Enums.Document.Extension DocumentFormat(string mimeFormat);
        Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId);
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
        Task DeleteDocumentProperty(long documentPropertyId);
    }
}