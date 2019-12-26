using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class ProfileAttributes
    {
        public ProfileAttributes()
        {
            Properties = new HashSet<Properties>();
        }

        public long ProfileAttributeId { get; set; }
        public string Name { get; set; }
        public int NameX { get; set; }
        public int NameY { get; set; }
        public short NameWidth { get; set; }
        public short NameHeight { get; set; }
        public byte PaperSizeId { get; set; }
        public byte BlockPositionId { get; set; }
        public int ProfileVersionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Value { get; set; }
        public int? ValueX { get; set; }
        public int? ValueY { get; set; }
        public short? ValueWidth { get; set; }
        public short? ValueHeight { get; set; }
        public short? StandardAttributeId { get; set; }

        public virtual BlockPositions BlockPosition { get; set; }
        public virtual PaperSizes PaperSize { get; set; }
        public virtual ProfileVersions ProfileVersion { get; set; }
        public virtual StandardAttributes StandardAttribute { get; set; }
        public virtual ICollection<Properties> Properties { get; set; }
    }
}
