﻿using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class DrawingLayers
    {
        public long DrawingLayerId { get; set; }
        public string Name { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawings Drawing { get; set; }
    }
}
