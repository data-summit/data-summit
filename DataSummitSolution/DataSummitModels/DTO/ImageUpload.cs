using DataSummitModels.DB;
using System.Collections.Generic;

namespace DataSummitModels.DTO
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
        public List<TemplateAttribute> TemplateAttributes { get; set; }
        public List<ImageGrid> SplitImages { get; set; }
        public Paper.Types PaperSize { get; set; }

        public ImageUpload()
        { }

        public Document ToDocument()
        {
            Document draw = new Document
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
                TemplateVersionId = TemplateVersionId,
                ProjectId = ProjectId,
                Sentences = (ICollection<Sentence>)Sentences,
                ImageGrids = SplitImages,
                Success = Success,
                Type = Type.ToString(),
                Tasks = (ICollection<Task>)Tasks,
                UserId = UserId
            };
            return draw;
        }
    }
}
