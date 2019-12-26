using System;
using System.Collections.Generic;

namespace Models
{
    public interface IDrawingBase
    {
        long DrawingId { get; set; }
        string FileName { get; set; }
        string BlobURL { get; set; }
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
        long? UserId { get; set; }
        string Type { get; set; }
        List<TaskDuration> Tasks { get; set; }
    }
}
