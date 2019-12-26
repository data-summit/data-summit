using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class DrawingFeatures
    {
        public DrawingFeatures()
        {
            //Points = new HashSet<Points>();
        }

        public long DrawingFeatureId { get; set; }
        public string Vendor { get; set; }
        public string Value { get; set; }
        public long DrawingId { get; set; }
        public long? Left { get; set; }
        public long? Top { get; set; }
        public long? Width { get; set; }
        public long? Height { get; set; }
        public byte? Feature { get; set; }
        public long? Center { get; set; }

        public virtual Drawings Drawing { get; set; }
        //public virtual ICollection<Points> Points { get; set; }
    }
}
