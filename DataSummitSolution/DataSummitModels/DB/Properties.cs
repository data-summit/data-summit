using System;

namespace DataSummitModels.DB
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long TemplateAttributeId { get; set; }

        public virtual TemplateAttributes TemplateAttribute { get; set; }
        public virtual Sentences Sentence { get; set; }
    }
}
