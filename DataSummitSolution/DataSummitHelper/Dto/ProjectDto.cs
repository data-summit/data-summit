using DataSummitModels.DB;
using System;

namespace DataSummitHelper.Interfaces
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }

        public ProjectDto()
        { ; }

        public ProjectDto(Projects project)
        {
            ProjectId = project.ProjectId;
            ProjectName = project.Name;
            CompanyId = project.CompanyId;
            CreatedDate = project.CreatedDate;
        }

        public Projects ToProject() => new Projects
        {
            ProjectId = ProjectId,
            Name = ProjectName,
            CompanyId = CompanyId,
            CreatedDate = CreatedDate,
        };
    }
}