using DataSummitModels.DB;
using System;

namespace DataSummitService.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }

        public ProjectDto()
        { ; }

        public ProjectDto(Project project)
        {
            ProjectId = project.ProjectId;
            ProjectName = project.Name;
            CompanyId = project.CompanyId;
            CreatedDate = project.CreatedDate;
        }

        public Project ToProject() => new Project
        {
            ProjectId = ProjectId,
            Name = ProjectName,
            CompanyId = CompanyId,
            CreatedDate = CreatedDate,
        };
    }
}