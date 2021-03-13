using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class TemplateVersion
    {
        public TemplateVersion()
        {
            DocumentTemplates = new HashSet<DocumentTemplate>();
            Documents = new HashSet<Document>();
            TemplateAttributes = new HashSet<TemplateAttribute>();
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

        public virtual Company Company { get; set; }
        public virtual ICollection<DocumentTemplate> DocumentTemplates { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<TemplateAttribute> TemplateAttributes { get; set; }
    }
}
