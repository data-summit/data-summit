using System;
using System.Collections.Generic;

namespace AzureFunctions.Models
{
    public class ImageUpload
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public long DrawingId { get; set; }
        public string FileName { get; set; }
        public int WidthOriginal { get; set; }
        public int HeightOriginal { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Tasks> Tasks { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public string Type { get; set; }
        public List<string> Layers { get; set; }
        public string BlobURL { get; set; }
        public string ContainerName { get; set; }
        public bool Success { get; set; }
        public int ProfileVersionId { get; set; }
        public List<ProfileAttributes> ProfileAttributes { get; set; }
        public decimal? AzureConfidence { get; set; }
        public decimal? GoogleConfidence { get; set; }
        public decimal? AmazonConfidence { get; set; }
        public byte[] File { get; set; }
        public bool Processed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public List<ImageGrid> SplitImages { get; set; }
        public Paper.Type PaperSize { get; set; }
        public List<Sentences> Sentences { get; set; }

        public ImageUpload()
        { }
    }
}
