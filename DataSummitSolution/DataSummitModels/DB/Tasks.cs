using System;

namespace DataSummitModels.DB
{
    public partial class Tasks
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DocumentId { get; set; }

        public virtual Documents Document { get; set; }

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
