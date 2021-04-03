using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class UserInfoType
    {
        public UserInfoType()
        {
            UserInfos = new HashSet<UserInfo>();
        }

        public byte UserInfoTypeId { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
