using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitDao
    {
        #region Companies
        Task<List<DataSummitModels.DB.Company>> GetAllCompanies();
        Task<DataSummitModels.DB.Company> GetCompanyById(int id);
        System.Threading.Tasks.Task CreateCompany(DataSummitModels.DB.Company company);
        System.Threading.Tasks.Task UpdateCompany(DataSummitModels.DB.Company company);
        System.Threading.Tasks.Task DeleteCompany(int id);
        #endregion

        #region Projects
        Task<List<DataSummitModels.DB.Project>> GetAllCompanyProjects(int companyId);
        Task<DataSummitModels.DB.Project> GetProjectById(int id);
        System.Threading.Tasks.Task CreateProject(DataSummitModels.DB.Project company);
        System.Threading.Tasks.Task UpdateProjectName(DataSummitModels.DB.Project company);
        System.Threading.Tasks.Task DeleteProject(int id);
        #endregion

        #region Documents
        System.Threading.Tasks.Task CreateDocument(DataSummitModels.DB.Document document);
        void UpdateDocument(DataSummitModels.DB.Document document);
        void DeleteDocument(long documentId);
        void DeleteDocument(string documentUrl);
        void DeleteDocument(DataSummitModels.DB.Document document);
        System.Threading.Tasks.Task DeleteDocumentAsync(long documentId);
        System.Threading.Tasks.Task DeleteDocumentAsync(string documentUrl);
        void AddDocumentFeatures(List<DocumentFeature> documentFeatures);
        void AddDocumentFeature(DocumentFeature documentFeature);
        Task<List<DataSummitModels.DB.Document>> GetProjectDocuments(int companyId);
        Task<DataSummitModels.DB.Document> GetDocumentsByUrlAsync(string documentUrl);
        DataSummitModels.DB.Document GetDocumentsByUrl(string documentUrl);
        Task<List<DataSummitModels.DB.Document>> GetAllProjectDocuments(int projectId);
        #endregion

        #region Templates
        Task<List<DataSummitModels.DB.TemplateVersion>> GetCompanyTemplateVersions(int companyId);
        Task<List<DataSummitModels.DB.TemplateVersion>> GetProjectTemplateVersions(int projectId);
        #endregion

        #region Template Attributes
        Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttribtesForTemplateId(int templateId);
        System.Threading.Tasks.Task DeleteTemplateAttribute(long templateAttributeId);
        Task<List<DataSummitModels.DB.TemplateAttribute>> GetAttributesForDocumentId(int documentId);
        #endregion

        #region Properties
        Task<DataSummitModels.DB.Property> GetPropertyById(int id);
        Task<List<DocumentProperty>> GetDocumentPropertiesByDocumentId(int documentId);
        System.Threading.Tasks.Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue);
        Task<bool> DeleteProperty(long propertyId);
        #endregion

        #region Azure URLs
        Task<Tuple<string, string>> GetAzureUrlByNameAsync(string name);
        Tuple<string, string> GetAzureUrlByName(string name);
        #endregion

        #region ML URLs
        Task<AzureMLResource> GetMLUrlByNameAsync(string name);
        AzureMLResource GetMLUrlByName(string name);
        #endregion

    }
}
