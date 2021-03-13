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

        public ProjectDto(DataSummitModels.DB.Project project)
        {
            ProjectId = project.ProjectId;
            ProjectName = project.Name;
            CompanyId = project.CompanyId;
            CreatedDate = project.CreatedDate;
        }

        public DataSummitModels.DB.Project ToProject() => new DataSummitModels.DB.Project
        {
            ProjectId = ProjectId,
            Name = ProjectName,
            CompanyId = CompanyId,
            CreatedDate = CreatedDate,
        };
    }
}