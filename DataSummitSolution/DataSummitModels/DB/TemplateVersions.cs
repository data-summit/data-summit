using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class TemplateVersions
    {
        public TemplateVersions()
        {
            TemplateAttributes = new HashSet<TemplateAttributes>();
        }

        public int TemplateVersionId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public byte[] Image { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public int? WidthOriginal { get; set; }
        public int? HeightOriginal { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<TemplateAttributes> TemplateAttributes { get; set; }
    }
}
