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
        Task<List<Companies>> GetAllCompanies();
        Task<Companies> GetCompanyById(int id);
        Task CreateCompany(Companies company);
        Task UpdateCompany(Companies company);
        Task DeleteCompany(int id);
        #endregion

        #region Projects
        Task<List<Projects>> GetAllCompanyProjects(int companyId);
        Task<Projects> GetProjectById(int id);
        Task CreateProject(Projects company);
        Task UpdateProjectName(Projects company);
        Task DeleteProject(int id);
        #endregion

        Task<List<Documents>> GetProjectDocuments(int companyId);

        #region Templates
        Task<List<TemplateVersions>> GetCompanyTemplateVersions(int companyId);
        Task<List<TemplateVersions>> GetProjectTemplateVersions(int projectId);
        #endregion


        Task<List<TemplateAttributes>> GetTemplateAttribtesForTemplateId(int templateId);

        Task DeleteTemplateAttribute(long templateAttributeId);
        Task<List<Documents>> GetAllProjectDocuments(int projectId);
        Task<List<DocumentProperty>> GetDocumentPropertiesByDocumentId(int documentId);
        Task UpdateDocumentPropertyValue(Guid documentPropertyId, string documentPropertyValue);
        Task<List<TemplateAttributes>> GetTemplateAttributesForDocumentId(int documentId);
    }
}
