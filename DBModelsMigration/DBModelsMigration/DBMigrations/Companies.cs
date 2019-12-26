using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Companies
    {
        public Companies()
        {
            Addresses = new HashSet<Addresses>();
            AspNetUsers = new HashSet<AspNetUsers>();
            AzureCompanyResourceUrls = new HashSet<AzureCompanyResourceUrls>();
            Orders = new HashSet<Orders>();
            ProfileVersions = new HashSet<ProfileVersions>();
            Projects = new HashSet<Projects>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string CompanyNumber { get; set; }
        public string Vatnumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Website { get; set; }

        public virtual ICollection<Addresses> Addresses { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<AzureCompanyResourceUrls> AzureCompanyResourceUrls { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<ProfileVersions> ProfileVersions { get; set; }
        public virtual ICollection<Projects> Projects { get; set; }
    }
}
