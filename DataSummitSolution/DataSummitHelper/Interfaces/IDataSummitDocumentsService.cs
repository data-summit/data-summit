using DataSummitService.Dto;
using DataSummitModels.DB;
using DataSummitModels.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitDocumentsService
    {
        DocumentContentType DocumentType(string mimeType);
        DocumentExtension DocumentFormat(string mimeFormat);
        DocumentDto GetDocumentDtoByUrl(string documentUrl);
        Document GetDocumentByUrl(string documentUrl);
        Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId);
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
        void UpdateDocument(Document document);
        Task UpdateDocumentFeature(string documentUrl);
        Task DeleteDocumentProperty(long documentPropertyId);
    }
}