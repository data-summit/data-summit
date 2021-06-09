using DataSummitService.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Dao.Interfaces
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
        Task ReplaceDocumentFeatures(string documentUrl, List<DocumentFeature> features);

        Task<List<Document>> GetProjectDocuments(int companyId);
        Task<Document> GetDocumentsByUrlAsync(string documentUrl);
        Document GetDocumentByUrl(string documentUrl);
        Task<List<Document>> GetAllProjectDocuments(int projectId);
        #endregion
    }
}
