using System;
using System.Collections.Generic;
using System.Text;
using TemplateMapper.OCR.Consolidated;

namespace Models
{
    public class ImageUpload : DrawingBase
    {
        public int CompanyId { get; set; }
        public int WidthOriginal { get; set; }
        public int HeightOriginal { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public string BlobStorageName { get; set; }
        public string BlobStorageKey { get; set; }
        public List<string> Layers { get; set; }
        public List<ImageGrid> SplitImages { get; set; }
        public List<Sentence> Sentences { get; set; }
        public ImageUpload()
        { }

        public Drawing ToDrawing()
        {
            Drawing draw = new Drawing
            {
                AmazonConfidence = AmazonConfidence,
                AzureConfidence = AzureConfidence,
                BlobURL = BlobURL,
                ContainerName = ContainerName,
                CreatedDate = CreatedDate,
                DrawingId = DrawingId,
                File = File,
                FileName = FileName,
                GoogleConfidence = GoogleConfidence,
                Processed = Processed,
                ProfileVersionId = ProfileVersionId,
                ProjectId = ProjectId,
                Success = Success,
                Type = Type,
                Tasks = Tasks,
                UserId = UserId
            };
            return draw;
        }
    }
}
