using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class BlockPosition
    {
        public BlockPosition()
        {
            TemplateAttributes = new HashSet<TemplateAttribute>();
        }

        public byte BlockPositionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<TemplateAttribute> TemplateAttributes { get; set; }
    }
}
