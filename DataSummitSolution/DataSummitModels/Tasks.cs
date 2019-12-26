using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class Tasks
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawings Drawing { get; set; }

        public Tasks()
        { }

        public Tasks(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }
    }
}
