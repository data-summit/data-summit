using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class PaperSizes
    {
        public PaperSizes()
        {
            Documents = new HashSet<Documents>();
            ProfileAttributes = new HashSet<ProfileAttributes>();
        }

        public byte PaperSizeId { get; set; }
        public string Name { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public decimal? PhysicalWidth { get; set; }
        public decimal? PhysicalHeight { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
        public virtual ICollection<ProfileAttributes> ProfileAttributes { get; set; }
    }
}
