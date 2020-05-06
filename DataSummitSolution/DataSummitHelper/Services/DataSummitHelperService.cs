
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitModels.DB;

namespace DataSummitHelper.Services
{
    public class DataSummitHelperService : IDataSummitHelperService
    {
        private readonly IDataSummitDao _dao;

        public DataSummitHelperService(IDataSummitDao dao)
        {
            _dao = dao;
        }

        #region Companies
        public async Task<List<CompanyDto>> GetAllCompanies()
        {
            var companies = await _dao.GetAllCompanies();
            var companyDtos = companies.Select(c => new CompanyDto(c))
                .ToList();

            return companyDtos;
        }

        public async Task<CompanyDto> GetCompanyById(int id)
        {
            var  company = await _dao.GetCompanyById(id);
            return new CompanyDto(company);
        }

        public async Task CreateCompany(CompanyDto companyDto)
        {
            var company = companyDto.ToCompany();
            await _dao.CreateCompany(company);
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            await _dao.UpdateCompany(company.ToCompany());
        }

        public async Task DeleteCompany(int id)
        {
           await _dao.DeleteCompany(id);
        }
        #endregion

        #region Projects
        public async Task<List<ProjectDto>> GetAllCompanyProjects(int id)
        {
            var projects = await _dao.GetAllCompanyProjects(id);
            var projectDtos = projects.Select(p => new ProjectDto(p))
                .ToList();

            return projectDtos;
        }

        public async Task CreateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            project.BlobStorageName = "TODO: Where From";
            project.BlobStorageKey = "TODO: Where From";
            project.UserId = -1;
            await _dao.CreateProject(project);
        }

        public async Task UpdateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            await _dao.UpdateProjectName(project);
        }

        public async Task DeleteProject(int id)
        {
            await _dao.DeleteProject(id);
        }
        #endregion

        #region Drawings
        public async Task DeleteDrawingProperty(long drawingPropertyId)
        {
            await _dao.DeleteProfileAttribute(drawingPropertyId);
        }

        public async Task<List<DrawingDto>> GetDrawingsForProjectId(int projectId)
        {
            var drawings = await _dao.GetAllProjectDrawings(projectId);

            var drawingDtos = drawings.Select(d => new DrawingDto(d))
                .ToList();

            return drawingDtos;
        }

        public async Task<List<DrawingDto>> GetProjectDrawings(int projectId)
        {
            var drawings = await _dao.GetProjectDrawings(projectId);
            var drawingDtos = drawings.Select(d => new DrawingDto(d))
                .ToList();

            return drawingDtos;
        }
        #endregion

        #region Properties
        public async Task UpdateDrawingPropertyValue(DrawingPropertyDto drawingProperty)
        {
            await _dao.UpdateDrawingPropertyValue(drawingProperty.SentenceId, drawingProperty.WordValue);
        }

        public async Task<List<DrawingPropertyDto>> GetDrawingProperties(int drawingId)
        {
            var drawingProperties = await _dao.GetDrawingPropertiesByDrawingId(drawingId);
            var drawingPropertyDtos = drawingProperties.Select(d => new DrawingPropertyDto(d))
                .ToList();

            return drawingPropertyDtos;
        }
        #endregion

        #region Templates
        public async Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId)
        {
            var templates = await _dao.GetCompanyProfileVersions(companyId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDto>> GetAllProjectTemplates(int projectId)
        {
            var templates = await _dao.GetProjectProfileVersions(projectId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateAttributeDto>> GetTemplateAttribtes(int templateId)
        {
            var templateAttributes = await _dao.GetTemplateAttribtesForTemplateId(templateId);
            var templateAttributeDtos = templateAttributes.Select(d => new TemplateAttributeDto(d))
                .ToList();

            return templateAttributeDtos;
        }
        #endregion

        public Uri GetIndividualUrl(int companyId, string azureResource)
        {
            throw new NotImplementedException();
        }
    }
}