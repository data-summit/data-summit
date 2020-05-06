﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        [StringLength(128)]
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string NormalizedName { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}