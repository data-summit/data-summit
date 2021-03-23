using DataSummitHelper.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitDocumentsService
    {
        Task<bool> IsDocument(int documentId);
        Task DeleteDocumentProperty(long documentPropertyId);
        Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId);
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
    }
}