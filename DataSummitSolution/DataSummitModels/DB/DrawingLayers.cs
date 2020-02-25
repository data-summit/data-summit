using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class DrawingLayers
    {
        public long DrawingLayerId { get; set; }
        public string Name { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawings Drawing { get; set; }
    }
}
