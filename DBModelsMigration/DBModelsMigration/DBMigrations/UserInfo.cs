using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class UserInfo
    {
        public long UserInfoId { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Id { get; set; }
        public byte? UserInfoTypeId { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual UserInfoTypes UserInfoType { get; set; }
    }
}
