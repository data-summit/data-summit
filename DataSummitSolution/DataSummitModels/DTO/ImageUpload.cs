using DataSummitModels.DB;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public class ImageUploadDto
    {
        public string ContainerName { get; set; }
        public string BlobUrl { get; set; }
        public List<ImageSectionDto> SplitImages { get; set; }

        public int CompanyId { get; set; }
        public int WidthOriginal { get; set; }
        public int HeightOriginal { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public List<TemplateAttribute> TemplateAttributes { get; set; }
        public Paper.PaperType PaperSize { get; set; }

        public string Validate()
        {
            //if (imgUpload.Tasks == null) imgUpload.Tasks = new List<DataSummitModels.DB.FunctionTask>();
            //if (imgUpload.Layers == null) imgUpload.Layers = new List<DocumentLayer>();

            //if (imgUpload.DocumentId < 0) return new BadRequestObjectResult("Illegal input: DocumentId is less than zero.");
            //if (imgUpload.FileName == "") return new BadRequestObjectResult("Illegal input: File name is ,less than zero.");
            if (string.IsNullOrEmpty(StorageAccountName)) { return "Illegal input: Storage name required."; };
            if (string.IsNullOrEmpty(StorageAccountKey)) { return "Illegal input: Storage key required."; };
            if (WidthOriginal <= 0) { return "Illegal input: Image must have width greater than zero"; };
            if (HeightOriginal <= 0) { return "Illegal input: Image must have height greater than zero"; };
            //if (imgUpload.ContainerName == "") return new BadRequestObjectResult("Illegal input: Container must have a GUID name");

            return null;
        }
    }
}
