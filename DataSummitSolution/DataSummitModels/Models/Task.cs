﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class Task
    {
        public long TaskId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TimeStamp { get; set; }
        public long DocumentId { get; set; }
        public TimeSpan Duration { get; set; }

        [ForeignKey("DocumentId")]
        [InverseProperty("Tasks")]
        public virtual Document Document { get; set; }
    }
}