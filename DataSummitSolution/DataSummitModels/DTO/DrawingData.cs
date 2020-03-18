using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitModels.DTO
{
    public class DrawingData
    {
        //From Drawing class
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

        //From ProfileVersion class (aka Template)
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? WidthOriginal { get; set; }
        public int? HeightOriginal { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

        public List<PropertyData> Properties { get; set; }

        public DrawingData()
        { }
    }
}
