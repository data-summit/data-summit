using DataSummitModels.DB;
using DataSummitModels.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace DataSummitModels.DTO
{
    public class ImageSectionDto
    {
        public string BlobUrl { get; set; }
        public string Name { get; set; }
        public int WidthStart { get; set; }
        public int HeightStart { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageType ImageType { get; set; } = ImageType.Unknown;
        public Image Image { get; set; }

        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        //public DocumentContentType DocumentType { get; set; } = DocumentContentType.Unknown;
    }
}