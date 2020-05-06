using DataSummitModels.DB;
using System;

namespace DataSummitHelper.Interfaces
{
    public class TemplateDto
    {
        public int ProfileVersionId { get; set; }
        public string TemplateName { get; set; }
        public int CompanyId { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public DateTime? CreatedDate { get; set; }

        public TemplateDto(ProfileVersions profileVersions)
        {
            ProfileVersionId = profileVersions.ProfileVersionId;
            TemplateName = profileVersions.Name;
            CompanyId = profileVersions.CompanyId;
            Height = profileVersions.Height;
            Width = profileVersions.Width;
            CreatedDate = profileVersions.CreatedDate;
        }
    }
}