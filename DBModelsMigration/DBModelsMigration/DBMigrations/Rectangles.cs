using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Rectangles
    {
        public Guid RectangleId { get; set; }
        public long Height { get; set; }
        public long Left { get; set; }
        public long Top { get; set; }
        public long Width { get; set; }
        public Guid SentenceId { get; set; }

        public virtual Sentences Sentence { get; set; }
    }
}
