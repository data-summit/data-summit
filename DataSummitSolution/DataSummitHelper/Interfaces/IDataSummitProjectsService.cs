using DataSummitService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IDataSummitProjectsService
    {
        Task<List<ProjectDto>> GetAllCompanyProjects(int id);
        Task CreateProject(ProjectDto project);
        Task UpdateProject(ProjectDto projectDto);
        Task DeleteProject(int id);
    }
}