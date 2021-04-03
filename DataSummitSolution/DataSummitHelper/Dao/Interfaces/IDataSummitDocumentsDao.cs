using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitDocumentsDao
    {
        #region Documents
        Task CreateDocument(Document document);
        void UpdateDocument(Document document);
        void DeleteDocument(long documentId);
        void DeleteDocument(string documentUrl);
        void DeleteDocument(Document document);
        Task DeleteDocumentAsync(long documentId);
        Task DeleteDocumentAsync(string documentUrl);
        void AddDocumentFeatures(List<DocumentFeature> documentFeatures);
        void AddDocumentFeature(DocumentFeature documentFeature);
        Task UpdateDocumentFeature(string documentUrl, DocumentFeature documentFeature);

        Task<List<Document>> GetProjectDocuments(int companyId);
        Task<Document> GetDocumentsByUrlAsync(string documentUrl);
        Document GetDocumentsByUrl(string documentUrl);
        Task<List<Document>> GetAllProjectDocuments(int projectId);
        #endregion
    }
}
