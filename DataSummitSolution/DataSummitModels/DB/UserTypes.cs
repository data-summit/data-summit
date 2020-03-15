using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class UserTypes
    {
        public UserTypes()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public byte UserTypeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
