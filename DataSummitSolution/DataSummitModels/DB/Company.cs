using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Company
    {
        public Company()
        {
            Addresses = new HashSet<Address>();
            AspNetUsers = new HashSet<AspNetUser>();
            AzureCompanyResourceUrls = new HashSet<AzureCompanyResourceUrl>();
            Orders = new HashSet<Order>();
            Projects = new HashSet<Project>();
            TemplateVersions = new HashSet<TemplateVersion>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string CompanyNumber { get; set; }
        public string Vatnumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Website { get; set; }
        public string ResourceGroup { get; set; }
        public string Region { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<AzureCompanyResourceUrl> AzureCompanyResourceUrls { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TemplateVersion> TemplateVersions { get; set; }
    }
}
