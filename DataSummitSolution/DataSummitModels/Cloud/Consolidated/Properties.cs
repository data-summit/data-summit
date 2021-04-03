using DataSummitModels.DB;
using System;
using System.Collections.Generic;

namespace DataSummitModels.Cloud.Consolidated
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long TemplateAttributeId { get; set; }

        public virtual TemplateAttribute TemplateAttribute { get; set; }
        public virtual Sentences Sentence { get; set; }

        public List<DB.Property> ToModel(List<Properties> props)
        {
            List<DB.Property> lp = new List<DB.Property>();
            foreach(Properties p in props)
            {
                lp.Add(p.ToModel());
            }
            return lp;
        }

        public DB.Property ToModel()
        {
            DB.Property p = new DB.Property();

            p.TemplateAttributeId = TemplateAttributeId;
            p.PropertyId = PropertyId;
            p.Sentence = Sentence.ToModel();
            p.SentenceId = SentenceId;

            return p;
        }
    }
}
