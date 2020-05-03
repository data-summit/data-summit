using DataSummitHelper.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IDataSummitHelperService
    {
        #region Companies
        Task<List<CompanyDto>> GetAllCompanies();
        Task<CompanyDto> GetCompanyById(int id);
        Task CreateCompany(CompanyDto companyDto);
        Task UpdateCompany(CompanyDto companyDto);
        Task DeleteCompany(int id);
        #endregion

        #region Projects
        Task<List<ProjectDto>> GetAllCompanyProjects(int id);
        Task CreateProject(ProjectDto project);
        Task UpdateProject(ProjectDto projectDto);
        Task DeleteProject(int id);
        #endregion

        #region Drawings
        Task<List<DrawingDto>> GetProjectDrawings(int projectId);
        Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId);
        Task DeleteDrawingProperty(long drawingPropertyId);
        Task<List<DrawingPropertyDto>> GetDrawingProperties(int drawingId);
        Task UpdateDrawingPropertyValue(DrawingPropertyDto drawingProperty);
        Uri GetIndividualUrl(int companyId, string azureResource);

        Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId);
        #endregion

        #region ProfileVersions

        #endregion


        #region Properties

        #endregion

    }
}