using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Consolidated
{
    public partial class Points
    {
        public Points()
        {
            SentencesHeightNavigation = new HashSet<Sentences>();
            SentencesLeftNavigation = new HashSet<Sentences>();
            SentencesTopNavigation = new HashSet<Sentences>();
            SentencesWidthNavigation = new HashSet<Sentences>();
        }

        public Guid PointId { get; set; }
        public long X { get; set; }
        public long Y { get; set; }

        public virtual ICollection<Sentences> SentencesHeightNavigation { get; set; }
        public virtual ICollection<Sentences> SentencesLeftNavigation { get; set; }
        public virtual ICollection<Sentences> SentencesTopNavigation { get; set; }
        public virtual ICollection<Sentences> SentencesWidthNavigation { get; set; }

        public Points(long x, long y)
        {
            X = x;
            Y = y;
        }
    }
}
