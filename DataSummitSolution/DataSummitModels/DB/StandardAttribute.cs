using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class StandardAttribute
    {
        public StandardAttribute()
        {
            TemplateAttributes = new HashSet<TemplateAttribute>();
        }

        public short StandardAttributeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TemplateAttribute> TemplateAttributes { get; set; }
    }
}
