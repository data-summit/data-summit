using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models
{
    public partial class Sentences
    {
        public Sentences()
        {
            Properties = new HashSet<Properties>();
        }

        public Guid SentenceId { get; set; }
        public string Words { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public string Vendor { get; set; }
        public bool IsUsed { get; set; }
        public decimal? Confidence { get; set; }
        public decimal? SlendernessRatio { get; set; }

        public virtual ICollection<Properties> Properties { get; set; }
    }
}
