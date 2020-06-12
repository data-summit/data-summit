
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace DataSummitHelper.Services
{
    public class DataSummitHelperService : IDataSummitHelperService
    {
        private readonly IDataSummitDao _dao;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public DataSummitHelperService(IDataSummitDao dao, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _dao = dao;
            _configuration = configuration;
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
            AzureResources azr = new AzureResources(_configuration);
            var company = companyDto.ToCompany();
            await _dao.CreateCompany(company);
            company.ResourceGroup = await azr.CreateResourceGroup(company.Name);
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            AzureResources azr = new AzureResources(_configuration);
            var companyDto = company.ToCompany();
            await _dao.UpdateCompany(companyDto);
            companyDto.ResourceGroup = await azr.UpdateResourceGroup(companyDto.ResourceGroup, companyDto.Name);
        }

        public async Task DeleteCompany(int id)
        {
            AzureResources azr = new AzureResources(_configuration);
            var company = await _dao.GetCompanyById(id);
            await _dao.DeleteCompany(id);
            await azr.DeleteResourceGroup(company.ResourceGroup);
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
            AzureResources azr = new AzureResources(_configuration);
            var project = projectDto.ToProject();
            await azr.CreateStorageAccount(project.Company.ResourceGroup, project.Name);
            project.UserId = -1;
            await _dao.CreateProject(project);
        }

        public async Task UpdateProject(ProjectDto projectDto)
        {
            AzureResources azr = new AzureResources(_configuration);
            var project = projectDto.ToProject();
            var oldProject = _dao.GetProjectById(projectDto.ProjectId).Result;
            project.Name = await azr.UpdateStorageAccount(project.Company.ResourceGroup, oldProject.Name, project.Name);
            await _dao.UpdateProjectName(project);
        }

        public async Task DeleteProject(int id)
        {
            AzureResources azr = new AzureResources(_configuration);
            var project = await _dao.GetProjectById(id);
            var company = await _dao.GetCompanyById(project.CompanyId);
            await _dao.DeleteProject(id);
            await azr.DeleteStorageAccount(company.ResourceGroup, project.Name);
        }
        #endregion

        #region Drawings
        public async Task<bool> IsDrawingDocument(int drawingId)
        {
            var documentClassification = _configuration["DocumentClassification"];

            //TODO make http call to //"https://documentlayout.cognitiveservices.azure.com/" and pass data to ML project

            return false;
        }
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