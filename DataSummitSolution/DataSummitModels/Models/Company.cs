﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class Company
    {
        public Company()
        {
            Addresses = new HashSet<Address>();
            AspNetUsers = new HashSet<AspNetUser>();
            AzureCompanyResourceUrls = new HashSet<AzureCompanyResourceUrl>();
            Orders = new HashSet<Order>();
            ProfileVersions = new HashSet<ProfileVersion>();
            Projects = new HashSet<Project>();
        }

        public int CompanyId { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(8)]
        public string CompanyNumber { get; set; }
        [StringLength(9)]
        public string Vatnumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        [StringLength(2083)]
        public string Website { get; set; }

        [InverseProperty("Company")]
        public virtual ICollection<Address> Addresses { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<AzureCompanyResourceUrl> AzureCompanyResourceUrls { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<ProfileVersion> ProfileVersions { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}