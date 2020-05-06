﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class Sentence
    {
        public Sentence()
        {
            Properties = new HashSet<Property>();
        }

        public Guid SentenceId { get; set; }
        [Required]
        public string Words { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        [Required]
        [StringLength(63)]
        public string Vendor { get; set; }
        public bool IsUsed { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? Confidence { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? SlendernessRatio { get; set; }
        public long DrawingId { get; set; }
        public string ModifiedWords { get; set; }

        [ForeignKey("DrawingId")]
        [InverseProperty("Sentences")]
        public virtual Drawing Drawing { get; set; }
        [InverseProperty("Sentence")]
        public virtual ICollection<Property> Properties { get; set; }
    }
}