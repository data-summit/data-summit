using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class BlockPositions
    {
        public BlockPositions()
        {
            ProfileAttributes = new HashSet<ProfileAttributes>();
        }

        public byte BlockPositionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<ProfileAttributes> ProfileAttributes { get; set; }
    }
}
