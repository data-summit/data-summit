using DataSummitHelper.Interfaces;
using System;

namespace DataSummitWeb.Models
{
    /// <summary>
    /// </summary>
    public sealed class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }

        public static Project FromDto(ProjectDto dto) => new Project
        {
            ProjectId = dto.ProjectId,
            Name = dto.ProjectName,
            CompanyId = dto.CompanyId,
            CreatedDate = dto.CreatedDate
        };

        public ProjectDto ToDto() => new ProjectDto()
        {
            ProjectId = ProjectId,
            ProjectName = Name,
            CompanyId = CompanyId,
            CreatedDate = CreatedDate
        };
    }
}
