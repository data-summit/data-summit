using System;
using System.Collections.Generic;

namespace DataSummitModels.Cloud.Consolidated
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long TemplateAttributeId { get; set; }

        //public virtual TemplateAttributes TemplateAttribute { get; set; }
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

            p.TemplateAttributeId = TemplateAttributeId;
            p.PropertyId = PropertyId;
            p.Sentence = Sentence.ToModel();
            p.SentenceId = SentenceId;

            return p;
        }
    }
}
