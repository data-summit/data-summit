using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Points
    {
        public long PointId { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public long DrawingFeatureId { get; set; }

        public virtual DrawingFeatures DrawingFeature { get; set; }
    }
}
