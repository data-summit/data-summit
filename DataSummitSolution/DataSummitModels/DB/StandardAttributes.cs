using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class StandardAttributes
    {
        public StandardAttributes()
        {
            TemplateAttributes = new HashSet<TemplateAttributes>();
        }

        public short StandardAttributeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TemplateAttributes> TemplateAttributes { get; set; }
    }
}
