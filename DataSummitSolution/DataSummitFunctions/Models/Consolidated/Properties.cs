using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Consolidated
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long ProfileAttributeId { get; set; }

        //public virtual ProfileAttributes ProfileAttribute { get; set; }
        public virtual Sentences Sentence { get; set; }


    }
}
