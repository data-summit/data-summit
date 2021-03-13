using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Gender
    {
        public Gender()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            Employees = new HashSet<Employee>();
        }

        public byte GenderId { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
