using System.Collections.Generic;
using System.Drawing;

namespace DataSummitModels.DB
{
    public partial class ImageGrids : ImageGrid
    {
        public Image Image { get; set; }
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        public Enums.Document.Type DocumentType { get; set; } = Enums.Document.Type.Unknown;
        public Enums.Image.Type ImageType { get; set; } = Enums.Image.Type.Unknown;

        public ImageGrids()
        { ; }
    }
}