using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class UserType
    {
        public UserType()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public byte UserTypeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
