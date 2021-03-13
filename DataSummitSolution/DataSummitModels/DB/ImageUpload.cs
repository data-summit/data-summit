using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public class ImageUpload : DocumentBase
    {
        public int CompanyId { get; set; }
        public int WidthOriginal { get; set; }
        public int HeightOriginal { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public List<ProfileAttributes> ProfileAttributes { get; set; }
        public List<ImageGrids> SplitImages { get; set; }
        public Paper.Types PaperSize { get; set; }

        public ImageUpload()
        { }

        public Documents ToDocument()
        {
            Documents draw = new Documents
            {
                AmazonConfidence = AmazonConfidence,
                AzureConfidence = AzureConfidence,
                BlobUrl = BlobUrl,
                ContainerName = ContainerName,
                CreatedDate = CreatedDate,
                DocumentId = DocumentId,
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
