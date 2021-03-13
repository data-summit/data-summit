using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class BlockPositions
    {
        public BlockPositions()
        {
            TemplateAttributes = new HashSet<TemplateAttributes>();
        }

        public byte BlockPositionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<TemplateAttributes> TemplateAttributes { get; set; }
    }
}
