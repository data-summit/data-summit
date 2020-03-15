using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class Genders
    {
        public Genders()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Employees = new HashSet<Employees>();
        }

        public byte GenderId { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
