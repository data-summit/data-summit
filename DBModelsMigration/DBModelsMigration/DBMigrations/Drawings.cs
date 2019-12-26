using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Drawings
    {
        public Drawings()
        {
            DrawingFeatures = new HashSet<DrawingFeatures>();
            DrawingLayers = new HashSet<DrawingLayers>();
            ImageGrids = new HashSet<ImageGrids>();
            Sentences = new HashSet<Sentences>();
            TaskDurations = new HashSet<TaskDurations>();
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
        public byte? PaperSizeId { get; set; }
        public byte PaperOrientationId { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public byte[] File { get; set; }

        public virtual PaperOrientations PaperOrientation { get; set; }
        public virtual PaperSizes PaperSize { get; set; }
        public virtual Projects Project { get; set; }
        public virtual ICollection<DrawingFeatures> DrawingFeatures { get; set; }
        public virtual ICollection<DrawingLayers> DrawingLayers { get; set; }
        public virtual ICollection<ImageGrids> ImageGrids { get; set; }
        public virtual ICollection<Sentences> Sentences { get; set; }
        public virtual ICollection<TaskDurations> TaskDurations { get; set; }
    }
}
