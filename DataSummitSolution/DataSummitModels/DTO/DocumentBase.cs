using DataSummitModels.DB;
using DataSummitModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitModels.DTO
{
    public class DocumentBase : IDocumentBase
    {
        public DocumentBase()
        {
            Layers = new HashSet<DocumentLayer>().ToList();
            Sentences = new HashSet<Sentence>().ToList();
            Tasks = new HashSet<DB.FunctionTask>().ToList();
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
        public byte[] File { get; set; }
        public byte? PaperSizeId { get; set; }
        public byte PaperOrientationId { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public DocumentContentType Type { get; set; }
        public DocumentExtension Format { get; set; }


        public virtual List<DocumentLayer> Layers { get; set; }
        public virtual List<Sentence> Sentences { get; set; }
        public virtual List<DB.FunctionTask> Tasks { get; set; }
    }
}
