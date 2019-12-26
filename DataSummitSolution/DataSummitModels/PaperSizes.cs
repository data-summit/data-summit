using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class PaperSizes
    {
        public PaperSizes()
        {
            Drawings = new HashSet<Drawings>();
            ProfileAttributes = new HashSet<ProfileAttributes>();
        }

        public byte PaperSizeId { get; set; }
        public string Name { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public decimal? PhysicalWidth { get; set; }
        public decimal? PhysicalHeight { get; set; }

        public virtual ICollection<Drawings> Drawings { get; set; }
        public virtual ICollection<ProfileAttributes> ProfileAttributes { get; set; }
    }
}
