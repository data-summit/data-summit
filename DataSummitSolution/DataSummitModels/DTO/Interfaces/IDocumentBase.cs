using DataSummitModels.DB;
using System;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public interface IDocumentBase
    {
        long DocumentId { get; set; }
        string FileName { get; set; }
        string BlobUrl { get; set; }
        string ContainerName { get; set; }
        bool Success { get; set; }
        int ProjectId { get; set; }
        int TemplateVersionId { get; set; }
        decimal? AzureConfidence { get; set; }
        decimal? GoogleConfidence { get; set; }
        decimal? AmazonConfidence { get; set; }
        //IFormFile File { get; set; }
        byte[] File { get; set; }
        bool Processed { get; set; }
        DateTime? CreatedDate { get; set; }
        string UserId { get; set; }
        Enums.Document.Type Type { get; set; }
        List<DocumentLayer> Layers { get; set; }
        List<Sentence> Sentences { get; set; }
        List<Task> Tasks { get; set; }

    }
}
