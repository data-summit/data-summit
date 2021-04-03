using DataSummitModels.DB;
using System;

namespace DataSummitService.Dto
{
    public class TemplateDto
    {
        public int TemplateVersionId { get; set; }
        public string TemplateName { get; set; }
        public int CompanyId { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public DateTime? CreatedDate { get; set; }

        public TemplateDto(TemplateVersion templateVersions)
        {
            TemplateVersionId = templateVersions.TemplateVersionId;
            TemplateName = templateVersions.Name;
            CompanyId = templateVersions.CompanyId;
            Height = templateVersions.Height;
            Width = templateVersions.Width;
            CreatedDate = templateVersions.CreatedDate;
        }
    }
}