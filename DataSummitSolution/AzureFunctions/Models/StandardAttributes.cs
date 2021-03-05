using System.Collections.Generic;

namespace AzureFunctions.Models
{
    public partial class StandardAttributes
    {
        public StandardAttributes()
        {
            ProfileAttributes = new HashSet<ProfileAttributes>();
        }

        public short StandardAttributeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProfileAttributes> ProfileAttributes { get; set; }
    }
}
