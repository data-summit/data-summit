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

        public List<Models.Properties> ToModel(List<Properties> props)
        {
            List<Models.Properties> lp = new List<Models.Properties>();
            foreach(Properties p in props)
            {
                lp.Add(p.ToModel());
            }
            return lp;
        }

        public Models.Properties ToModel()
        {
            Models.Properties p = new Models.Properties();

            p.ProfileAttributeId = ProfileAttributeId;
            p.PropertyId = PropertyId;
            p.Sentence = Sentence.ToModel();
            p.SentenceId = SentenceId;

            return p;
        }
    }
}
