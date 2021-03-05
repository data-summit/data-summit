using System;

namespace AzureFunctions.Models
{
    public partial class Properties
    {
        public long PropertyId { get; set; }
        public Guid SentenceId { get; set; }
        public long ProfileAttributeId { get; set; }

        public virtual ProfileAttributes ProfileAttribute { get; set; }
        public virtual Sentences Sentence { get; set; }

        public Consolidated.Properties ToModelConsolidated()
        {
            Consolidated.Properties p = new Consolidated.Properties();
            p.ProfileAttributeId = ProfileAttributeId;
            p.PropertyId = PropertyId;
            p.Sentence = Sentence.ToModelConsolidated();
            p.SentenceId = SentenceId;

            return p;
        }
    }
}
