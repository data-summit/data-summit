using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Task
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DocumentId { get; set; }
        public TimeSpan Duration { get; set; }

        public virtual Document Document { get; set; }
    }
}
