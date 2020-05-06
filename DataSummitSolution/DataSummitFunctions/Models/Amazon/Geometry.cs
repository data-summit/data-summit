using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class Geometry
    {
        public BoundingBox BoundingBox { get; set; }
        public List<Point> Polygon { get; set; }

        public Geometry()
        { }
    }
}
