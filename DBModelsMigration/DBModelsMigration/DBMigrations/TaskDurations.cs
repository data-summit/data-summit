using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class TaskDurations
    {
        public long TaskDurationId { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DrawingId { get; set; }
        public TimeSpan Duration { get; set; }

        public virtual Drawings Drawing { get; set; }
    }
}
