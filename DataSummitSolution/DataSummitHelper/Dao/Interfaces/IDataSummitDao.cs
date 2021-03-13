using DataSummitHelper.Classes;
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

        Task<List<DataSummitModels.DB.Document>> GetProjectDocuments(int companyId);

        #region Templates
        Task<List<DataSummitModels.DB.TemplateVersion>> GetCompanyTemplateVersions(int companyId);
        Task<List<DataSummitModels.DB.TemplateVersion>> GetProjectTemplateVersions(int projectId);
        #endregion


        Task<List<DataSummitModels.DB.TemplateAttribute>> GetTemplateAttribtesForTemplateId(int templateId);

        System.Threading.Tasks.Task DeleteTemplateAttribute(long templateAttributeId);
        Task<List<DataSummitModels.DB.Document>> GetAllProjectDocuments(int projectId);
        Task<List<DocumentProperty>> GetDocumentPropertiesByDocumentId(int documentId);
        System.Threading.Tasks.Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue);
        Task<List<DataSummitModels.DB.TemplateAttribute>> GetTemplateAttributesForDocumentId(int documentId);
    }
}
