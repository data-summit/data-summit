using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public interface IDrawingBase
    {
        long DrawingId { get; set; }
        string FileName { get; set; }
        string BlobUrl { get; set; }
        string ContainerName { get; set; }
        bool Success { get; set; }
        int ProjectId { get; set; }
        int ProfileVersionId { get; set; }
        decimal? AzureConfidence { get; set; }
        decimal? GoogleConfidence { get; set; }
        decimal? AmazonConfidence { get; set; }
        //IFormFile File { get; set; }
        byte[] File { get; set; }
        bool Processed { get; set; }
        DateTime? CreatedDate { get; set; }
        string UserId { get; set; }
        string Type { get; set; }
        List<DrawingLayers> Layers { get; set; }
        List<Sentences> Sentences { get; set; }
        List<Tasks> Tasks { get; set; }

    }
}
