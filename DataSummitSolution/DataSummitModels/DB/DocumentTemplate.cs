using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class DocumentTemplate
    {
        public int DocumentTemplateId { get; set; }
        public long? DocumentId { get; set; }
        public int? TemplateVersionId { get; set; }

        public virtual Document Document { get; set; }
        public virtual TemplateVersion TemplateVersion { get; set; }
    }
}
