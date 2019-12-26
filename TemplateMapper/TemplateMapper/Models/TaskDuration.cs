using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TaskDuration
    {
        public long TaskDurationId { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawing Drawing { get; set; }

        public TaskDuration()
        { }

        public TaskDuration(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }
    }
}
