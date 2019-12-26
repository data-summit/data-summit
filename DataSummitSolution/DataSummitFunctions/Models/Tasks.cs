using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models
{
    public class Tasks
    {
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime TimeStamp { get; set; }
        public long DrawingId { get; set; }

        public Tasks()
        { }

        public Tasks(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }

        //public Tasks(string name, string previous)
        //{
        //    Name = name;
        //    DateTime now = DateTime.Now;
        //    TimeStamp = now.ToLongDateString();
        //    Duration = now - DateTime.Parse(previous);
        //}
    }
}
