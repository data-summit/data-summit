using System;
using System.Collections.Generic;

namespace DataSummitModels.Cloud.Consolidated
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long ProfileAttributeId { get; set; }

        //public virtual ProfileAttributes ProfileAttribute { get; set; }
        public virtual Sentences Sentence { get; set; }

        public List<DB.Properties> ToModel(List<Properties> props)
        {
            List<DB.Properties> lp = new List<DB.Properties>();
            foreach(Properties p in props)
            {
                lp.Add(p.ToModel());
            }
            return lp;
        }

        public DB.Properties ToModel()
        {
            DB.Properties p = new DB.Properties();

            p.ProfileAttributeId = ProfileAttributeId;
            p.PropertyId = PropertyId;
            p.Sentence = Sentence.ToModel();
            p.SentenceId = SentenceId;

            return p;
        }
    }
}
