using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Point
    {
        public Guid PointId { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }
}
