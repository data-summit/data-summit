using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class ImageGrids
    {
        public long ImageGridId { get; set; }
        public string Name { get; set; }
        public int WidthStart { get; set; }
        public int HeightStart { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string BlobUrl { get; set; }
        public bool Processed { get; set; }
        public byte Type { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawings Drawing { get; set; }
    }
}
