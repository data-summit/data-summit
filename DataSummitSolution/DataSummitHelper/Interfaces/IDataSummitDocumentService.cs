using DataSummitHelper.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitDocumentService
    {
        #region Documents
        Task<List<DocumentDto>> GetProjectDocuments(int projectId);
        Task DeleteDocumentProperty(long documentPropertyId);
        Task<List<DocumentPropertyDto>> GetDocumentProperties(int documentId);
        Task UpdateDocumentPropertyValue(DocumentPropertyDto documentProperty);
        #endregion
    }
}