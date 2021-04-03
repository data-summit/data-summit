using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;

namespace DataSummitService.Services
{
    public class DataSummitProjectsService : IDataSummitProjectsService
    {
        private readonly IDataSummitProjectsDao _projectsDao;
        private readonly IDataSummitCompaniesDao _companiesDao;
        private readonly IAzureResourcesService _azureResources;

        public DataSummitProjectsService(IDataSummitProjectsDao projectsDao,
                                         IDataSummitCompaniesDao _companiesDao,
                                         IAzureResourcesService azureResources)
        {
            _projectsDao = projectsDao;
            _azureResources = azureResources;
        }
        public async Task<List<ProjectDto>> GetAllCompanyProjects(int id)
        {
            var projects = await _projectsDao.GetAllCompanyProjects(id);
            var projectDtos = projects.Select(p => new ProjectDto(p))
                .ToList();

            return projectDtos;
        }

        public async Task CreateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            await _azureResources.CreateStorageAccount(project.Company.ResourceGroup, project.Name);
            project.UserId = -1;
            await _projectsDao.CreateProject(project);
        }

        public async Task UpdateProject(ProjectDto projectDto)
        {
            var project = projectDto.ToProject();
            var oldProject = _projectsDao.GetProjectById(projectDto.ProjectId).Result;
            project.Name = await _azureResources.UpdateStorageAccount(project.Company.ResourceGroup, oldProject.Name, project.Name);
            await _projectsDao.UpdateProjectName(project);
        }

        public async Task DeleteProject(int id)
        {
            var project = await _projectsDao.GetProjectById(id);
            var company = await _companiesDao.GetCompanyById(project.CompanyId);
            await _projectsDao.DeleteProject(id);
            await _azureResources.DeleteStorageAccount(company.ResourceGroup, project.Name);
        }
    }
}