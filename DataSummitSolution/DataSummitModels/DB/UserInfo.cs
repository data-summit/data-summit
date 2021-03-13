using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class UserInfo
    {
        public long UserInfoId { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Id { get; set; }
        public byte? UserInfoTypeId { get; set; }

        public virtual AspNetUser IdNavigation { get; set; }
        public virtual UserInfoType UserInfoType { get; set; }
    }
}
