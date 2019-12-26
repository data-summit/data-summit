using System;
using System.Collections.Generic;

namespace Models
{
    public partial class DrawingLayer
    {
        public long DrawingLayerId { get; set; }
        public string Name { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawing Drawing { get; set; }
    }
}
