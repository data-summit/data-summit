using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitModels
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
        public List<ImageGrids> SplitImages { get; set; }
        public Paper.Types PaperSize { get; set; }
        public ImageUpload()
        { }

        public Drawings ToDrawing()
        {
            Drawings draw = new Drawings
            {
                AmazonConfidence = AmazonConfidence,
                AzureConfidence = AzureConfidence,
                BlobUrl = BlobUrl,
                ContainerName = ContainerName,
                CreatedDate = CreatedDate,
                DrawingId = DrawingId,
                //DrawingFeatures = DrawingFeatures,
                //Layers = DrawingLayers,
                File = File,
                FileName = FileName,
                GoogleConfidence = GoogleConfidence,
                PaperOrientationId = PaperOrientationId,
                PaperSizeId = PaperSizeId,
                Processed = Processed,
                ProfileVersionId = ProfileVersionId,
                ProjectId = ProjectId,
                Sentences = Sentences,
                ImageGrids = SplitImages,
                Success = Success,
                Type = Type,
                Tasks = Tasks,
                UserId = UserId
            };
            return draw;
        }
    }
}
