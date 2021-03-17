
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Dto;
using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using Microsoft.Extensions.Configuration;

namespace DataSummitHelper.Services
{
    public class DataSummitHelperService : IDataSummitHelperService
    {
        private readonly IDataSummitDao _dao;
        private readonly IAzureResources _azureResources;
        private readonly IConfiguration _configuration;

        public DataSummitHelperService(IDataSummitDao dao, IConfiguration configuration, IAzureResources azureResources)
        {
            _dao = dao;
            _configuration = configuration;
            _azureResources = azureResources;
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

        public async System.Threading.Tasks.Task CreateCompany(CompanyDto companyDto)
        {
            var company = companyDto.ToCompany();
            await _dao.CreateCompany(company);
            company.ResourceGroup = await _azureResources.CreateResourceGroup(company.Name);
        }

        public async System.Threading.Tasks.Task UpdateCompany(CompanyDto company)
        {
            var companyDto = company.ToCompany();
            await _dao.UpdateCompany(companyDto);
            companyDto.ResourceGroup = await _azureResources.UpdateResourceGroup(companyDto.ResourceGroup, companyDto.Name);
        }

        public async System.Threading.Tasks.Task DeleteCompany(int id)
        {
            var company = await _dao.GetCompanyById(id);
            await _dao.DeleteCompany(id);
            await _azureResources.DeleteResourceGroup(company.ResourceGroup);
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

        public async System.Threading.Tasks.Task CreateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            await _azureResources.CreateStorageAccount(project.Company.ResourceGroup, project.Name);
            project.UserId = -1;
            await _dao.CreateProject(project);
        }

        public async System.Threading.Tasks.Task UpdateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            var oldProject = _dao.GetProjectById(projectDto.ProjectId).Result;
            project.Name = await _azureResources.UpdateStorageAccount(project.Company.ResourceGroup, oldProject.Name, project.Name);
            await _dao.UpdateProjectName(project);
        }

        public async System.Threading.Tasks.Task DeleteProject(int id)
        {
            var project = await _dao.GetProjectById(id);
            var company = await _dao.GetCompanyById(project.CompanyId);
            await _dao.DeleteProject(id);
            await _azureResources.DeleteStorageAccount(company.ResourceGroup, project.Name);
        }
        #endregion

        #region Documents
        public async Task<bool> IsDocument(int documentId)
        {
            var documentClassification = _configuration["DocumentClassification"];

            //TODO make http call to //"https://documentlayout.cognitiveservices.azure.com/" and pass data to ML project

            return false;
        }
        public async System.Threading.Tasks.Task DeleteDocumentProperty(long documentPropertyId)
        {
            await _dao.DeleteTemplateAttribute(documentPropertyId);
        }

        public async Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId)
        {
            var documents = await _dao.GetAllProjectDocuments(projectId);

            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task<List<DocumentDto>> GetProjectDocuments(int projectId)
        {
            var documents = await _dao.GetProjectDocuments(projectId);
            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }
        #endregion

        #region Properties
        public async System.Threading.Tasks.Task UpdateDocumentPropertyValue(DocumentPropertyDto documentProperty)
        {
            await _dao.UpdateDocumentPropertyValue(documentProperty.SentenceId, documentProperty.WordValue);
        }

        public async Task<List<DocumentPropertyDto>> GetDocumentProperties(int documentId)
        {
            var documentProperties = await _dao.GetDocumentPropertiesByDocumentId(documentId);
            var documentPropertyDtos = documentProperties.Select(d => new DocumentPropertyDto(d))
                .ToList();

            return documentPropertyDtos;
        }
        #endregion

        #region Templates
        public async Task<List<TemplateDto>> GetAllCompanyTemplates(int companyId)
        {
            var templates = await _dao.GetCompanyTemplateVersions(companyId);
            var templateDtos = templates.Select(t => new TemplateDto(t))
                .ToList();

            return templateDtos;
        }

        public async Task<List<TemplateDto>> GetAllProjectTemplates(int projectId)
        {
            var templates = await _dao.GetProjectTemplateVersions(projectId);
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

        #region Secrets
        public string GetSecret(string secretName)
        {
            string key = "";
            try
            {
                key = _configuration[secretName];
            }
            catch (Exception)
            {
                return "";
            }
            return key;
        }
        #endregion

        public async Task<HttpResponseMessage> ProcessCall(Uri uri, string payload)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var stringTask = await client.PostAsync(uri, httpContent);
            return stringTask;
        }
    }
}