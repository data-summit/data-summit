using System;
using System.Collections.Generic;
using System.Linq;

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

        public Consolidated.Sentences ToModelConsolidated()
        {
            Consolidated.Sentences s = new Consolidated.Sentences();
            s.Confidence = Confidence;
            //s.DrawingId = DrawingId;  DrawingId doesn't exist in 'Models.Sentences
            s.Height = Height;
            s.IsUsed = IsUsed;
            s.Left = Left;
            s.Properties = Properties.Select(p => p.ToModelConsolidated()).ToList();
            s.SentenceId = SentenceId;
            if (SlendernessRatio != null) s.SlendernessRatio = (decimal)SlendernessRatio;
            s.Top = Top;
            s.Vendor = Vendor;
            s.Width = Width;
            s.Words = Words;

            return s;
        }
    }
}
