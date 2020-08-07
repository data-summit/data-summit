using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diagnostics.Logger
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime End { get; set; }
        public SourceEnum SourceType { get; set; }
        public string Source { get; set; }

        public Event()
        { }

        public Guid New(string message, SourceEnum sourceArea)
        {
            Event e = new Event();
            try
            {
                e.Id = Guid.NewGuid();
                e.Message = message;
                e.Start = DateTime.Now;
                e.SourceType = sourceArea;
                Log.Events.Add(e);
                if (Log.Events.Count(ev => ev == null) > 0)
                { }
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
            return e.Id;
        }

        public Guid New(string message, string source)
        {
            Event e = new Event();
            try
            {
                e.Id = Guid.NewGuid();
                e.Message = message;
                e.Start = DateTime.Now;
                e.Source = source;
                Log.Events.Add(e);
                if (Log.Events.Count(ev => ev == null) > 0)
                { }
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
            return e.Id;
        }

        public void Complete(Guid id)
        {
            try
            {
                if (Log.Events.Count(e => e != null && e.Id == id) > 0)
                {
                    Event e = Log.Events.First(ep => ep.Id == id);
                    e.End = DateTime.Now;
                    e.Duration = e.End.Subtract(e.Start);
                    Console.WriteLine(e.SourceType.ToString() + ": Completed " + e.Message + ": " + e.Duration.ToString("hh\\:mm\\:ss\\.fff", null));
                }
            }
            catch (Exception ae)
            {
                Log.WriteLine(ae.Message);
                if (ae.InnerException != null) Log.WriteLine(ae.InnerException.ToString());
            }
        }

        public enum SourceEnum
        {
            Error = 1,
            Warning = 2,
            SpecificSource = 3
        }
    }
}
