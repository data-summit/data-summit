using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Document
    {
        public Document()
        {
            DocumentFeatures = new HashSet<DocumentFeature>();
            DocumentLayers = new HashSet<DocumentLayer>();
            DocumentTemplates = new HashSet<DocumentTemplate>();
            ImageGrids = new HashSet<ImageGrid>();
            Sentences = new HashSet<Sentence>();
            Tasks = new HashSet<FunctionTask>();
        }

        public long DocumentId { get; set; }
        public string FileName { get; set; }
        public string BlobUrl { get; set; }
        public string ContainerName { get; set; }
        public bool Success { get; set; }
        public int ProjectId { get; set; }
        public int TemplateVersionId { get; set; }
        public decimal? AzureConfidence { get; set; }
        public decimal? GoogleConfidence { get; set; }
        public decimal? AmazonConfidence { get; set; }
        public byte? PaperSizeId { get; set; }
        public byte PaperOrientationId { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public byte[] File { get; set; }
        public byte DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual PaperOrientation PaperOrientation { get; set; }
        public virtual PaperSize PaperSize { get; set; }
        public virtual Project Project { get; set; }
        public virtual TemplateVersion TemplateVersion { get; set; }
        public virtual ICollection<DocumentFeature> DocumentFeatures { get; set; }
        public virtual ICollection<DocumentLayer> DocumentLayers { get; set; }
        public virtual ICollection<DocumentTemplate> DocumentTemplates { get; set; }
        public virtual ICollection<ImageGrid> ImageGrids { get; set; }
        public virtual ICollection<Sentence> Sentences { get; set; }
        public virtual ICollection<FunctionTask> Tasks { get; set; }
    }
}
