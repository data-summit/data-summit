using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class PaperSize
    {
        public PaperSize()
        {
            Documents = new HashSet<Document>();
            TemplateAttributes = new HashSet<TemplateAttribute>();
        }

        public byte PaperSizeId { get; set; }
        public string Name { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public decimal? PhysicalWidth { get; set; }
        public decimal? PhysicalHeight { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<TemplateAttribute> TemplateAttributes { get; set; }
    }
}
