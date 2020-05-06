﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class ProfileVersion
    {
        public ProfileVersion()
        {
            DrawingTemplates = new HashSet<DrawingTemplate>();
            Drawings = new HashSet<Drawing>();
            ProfileAttributes = new HashSet<ProfileAttribute>();
        }

        public int ProfileVersionId { get; set; }
        [Required]
        [StringLength(1023)]
        public string Name { get; set; }
        public int CompanyId { get; set; }
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public int? WidthOriginal { get; set; }
        public int? HeightOriginal { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("ProfileVersions")]
        public virtual Company Company { get; set; }
        [InverseProperty("ProfileVersion")]
        public virtual ICollection<DrawingTemplate> DrawingTemplates { get; set; }
        [InverseProperty("ProfileVersion")]
        public virtual ICollection<Drawing> Drawings { get; set; }
        [InverseProperty("ProfileVersion")]
        public virtual ICollection<ProfileAttribute> ProfileAttributes { get; set; }
    }
}