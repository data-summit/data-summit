using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class DocumentFeature
    {
        public long DocumentFeatureId { get; set; }
        public string Vendor { get; set; }
        public string Value { get; set; }
        public long DocumentId { get; set; }
        public decimal? Left { get; set; }
        public decimal? Top { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string Feature { get; set; }
        public decimal Confidence { get; set; }

        public virtual Document Document { get; set; }
    }
}
