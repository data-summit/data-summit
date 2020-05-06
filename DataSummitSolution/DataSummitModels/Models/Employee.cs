﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class Employee
    {
        public long EmployeeId { get; set; }
        [Required]
        [StringLength(31)]
        public string FirstName { get; set; }
        [StringLength(63)]
        public string MiddleNames { get; set; }
        [Required]
        [StringLength(31)]
        public string Surname { get; set; }
        [StringLength(51)]
        public string Title { get; set; }
        [StringLength(31)]
        public string TitleOfCourtesy { get; set; }
        [StringLength(63)]
        public string JobTitle { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }
        public byte? GenderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("GenderId")]
        [InverseProperty("Employees")]
        public virtual Gender Gender { get; set; }
    }
}