using DataSummitModels.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace DataSummitModels.DB
{
    public partial class ImageGrids : ImageGrid
    {
        public Image Image { get; set; }
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        public DocumentContentType DocumentType { get; set; } = DocumentContentType.Unknown;
        public ImageType ImageType { get; set; } = ImageType.Unknown;

        public ImageGrids()
        { ; }
    }
}