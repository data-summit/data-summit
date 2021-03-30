using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitDocumentsService
    {
        DataSummitModels.Enums.Document.Type DocumentType(string mimeType);
        DataSummitModels.Enums.Document.Extension DocumentFormat(string mimeFormat);
        DocumentDto GetDocumentDtoByUrl(string documentUrl);
        DataSummitModels.DB.Document GetDocumentByUrl(string documentUrl);
        Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId);
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
        void UpdateDocument(DataSummitModels.DB.Document document);
        Task DeleteDocumentProperty(long documentPropertyId);
    }
}