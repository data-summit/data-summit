using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;

namespace DataSummitHelper.Services
{
    public class DataSummitProjectsService : IDataSummitProjectsService
    {
        private readonly IDataSummitDao _dao;
        private readonly IAzureResourcesService _azureResources;

        public DataSummitProjectsService(IDataSummitDao dao, IAzureResourcesService azureResources)
        {
            _dao = dao;
            _azureResources = azureResources;
        }
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
    }
}