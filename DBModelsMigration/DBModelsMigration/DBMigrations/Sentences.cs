using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Sentences
    {
        public Sentences()
        {
            Properties = new HashSet<Properties>();
            Rectangles = new HashSet<Rectangles>();
        }

        public Guid SentenceId { get; set; }
        public string Words { get; set; }
        public Guid RectangleId { get; set; }
        public string Vendor { get; set; }
        public bool IsUsed { get; set; }
        public decimal? Confidence { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawings Drawing { get; set; }
        public virtual ICollection<Properties> Properties { get; set; }
        public virtual ICollection<Rectangles> Rectangles { get; set; }
    }
}
