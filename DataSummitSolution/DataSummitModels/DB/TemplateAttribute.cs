using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class TemplateAttribute
    {
        public TemplateAttribute()
        {
            Properties = new HashSet<Property>();
        }

        public long TemplateAttributeId { get; set; }
        public string Name { get; set; }
        public int NameX { get; set; }
        public int NameY { get; set; }
        public short NameWidth { get; set; }
        public short NameHeight { get; set; }
        public byte PaperSizeId { get; set; }
        public byte BlockPositionId { get; set; }
        public int TemplateVersionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Value { get; set; }
        public int? ValueX { get; set; }
        public int? ValueY { get; set; }
        public short? ValueWidth { get; set; }
        public short? ValueHeight { get; set; }
        public short? StandardAttributeId { get; set; }

        public virtual BlockPosition BlockPosition { get; set; }
        public virtual PaperSize PaperSize { get; set; }
        public virtual StandardAttribute StandardAttribute { get; set; }
        public virtual TemplateVersion TemplateVersion { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
    }
}
