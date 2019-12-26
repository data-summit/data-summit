using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class UserInfoTypes
    {
        public UserInfoTypes()
        {
            UserInfo = new HashSet<UserInfo>();
        }

        public byte UserInfoTypeId { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<UserInfo> UserInfo { get; set; }
    }
}
