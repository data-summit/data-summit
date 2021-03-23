using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitDocumentsService
    {
        DataSummitModels.Enums.Document.Type DocumentType(string mimeType);
        DataSummitModels.Enums.Document.Format DocumentFormat(string mimeFormat);
        Task DeleteDocumentProperty(long documentPropertyId);
        Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId);
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
    }
}