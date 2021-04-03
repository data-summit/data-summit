using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Property
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long TemplateAttributeId { get; set; }

        public virtual Sentence Sentence { get; set; }
        public virtual TemplateAttribute TemplateAttribute { get; set; }
    }
}
