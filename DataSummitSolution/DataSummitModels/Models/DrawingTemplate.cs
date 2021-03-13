﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSummitDbModels
{
    public partial class DocumentTemplate
    {
        public int DocumentTemplateId { get; set; }
        public long? DocumentId { get; set; }
        public int? ProfileVersionId { get; set; }

        [ForeignKey("DocumentId")]
        [InverseProperty("DocumentTemplates")]
        public virtual Document Document { get; set; }
        [ForeignKey("ProfileVersionId")]
        [InverseProperty("DocumentTemplates")]
        public virtual ProfileVersion ProfileVersion { get; set; }
    }
}