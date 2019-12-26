using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DrawingBase : IDrawingBase
    {
        public long DrawingId { get; set; }
        public string FileName { get; set; }
        public string BlobURL { get; set; }
        public string ContainerName { get; set; }
        public bool Success { get; set; }
        public int ProjectId { get; set; }
        public int ProfileVersionId { get; set; }
        public decimal? AzureConfidence { get; set; }
        public decimal? GoogleConfidence { get; set; }
        public decimal? AmazonConfidence { get; set; }
        public byte[] File { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Type { get; set; }
        public List<TaskDuration> Tasks { get; set; }
    }
}
