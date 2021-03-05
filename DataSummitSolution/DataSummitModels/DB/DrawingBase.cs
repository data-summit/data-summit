using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public class DrawingBase : IDrawingBase
    {
        public DrawingBase()
        {
            //Layers = new HashSet<DrawingLayers>();
            //Sentences = new HashSet<Sentences>();
            //Tasks = new HashSet<Tasks>();
        }

        public long DrawingId { get; set; }
        public string FileName { get; set; }
        public string BlobUrl { get; set; }
        public string ContainerName { get; set; }
        public bool Success { get; set; }
        public int ProjectId { get; set; }
        public int ProfileVersionId { get; set; }
        public decimal? AzureConfidence { get; set; }
        public decimal? GoogleConfidence { get; set; }
        public decimal? AmazonConfidence { get; set; }
        public byte[] File { get; set; }
        public byte? PaperSizeId { get; set; }
        public byte PaperOrientationId { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }


        public virtual List<DrawingLayers> Layers { get; set; }
        public virtual List<Sentences> Sentences { get; set; }
        public virtual List<Tasks> Tasks { get; set; }
    }
}
