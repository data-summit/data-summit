﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class TemplateAttribute
    {
        public TemplateAttribute()
        {
            Properties = new HashSet<Property>();
        }

        public long TemplateAttributeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int NameX { get; set; }
        public int NameY { get; set; }
        public short NameWidth { get; set; }
        public short NameHeight { get; set; }
        public byte PaperSizeId { get; set; }
        public byte BlockPositionId { get; set; }
        public int TemplateVersionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Value { get; set; }
        public int? ValueX { get; set; }
        public int? ValueY { get; set; }
        public short? ValueWidth { get; set; }
        public short? ValueHeight { get; set; }
        public short? StandardAttributeId { get; set; }

        [ForeignKey("BlockPositionId")]
        [InverseProperty("TemplateAttributes")]
        public virtual BlockPosition BlockPosition { get; set; }
        [ForeignKey("PaperSizeId")]
        [InverseProperty("TemplateAttributes")]
        public virtual PaperSize PaperSize { get; set; }
        [ForeignKey("TemplateVersionId")]
        [InverseProperty("TemplateAttributes")]
        public virtual TemplateVersion TemplateVersion { get; set; }
        [ForeignKey("StandardAttributeId")]
        [InverseProperty("TemplateAttributes")]
        public virtual StandardAttribute StandardAttribute { get; set; }
        [InverseProperty("TemplateAttribute")]
        public virtual ICollection<Property> Properties { get; set; }
    }
}