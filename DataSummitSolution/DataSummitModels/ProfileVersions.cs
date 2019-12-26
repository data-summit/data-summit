using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class ProfileVersions
    {
        public ProfileVersions()
        {
            ProfileAttributes = new HashSet<ProfileAttributes>();
        }

        public int ProfileVersionId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public byte[] Image { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public int? WidthOriginal { get; set; }
        public int? HeightOriginal { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<ProfileAttributes> ProfileAttributes { get; set; }
    }
}
