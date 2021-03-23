
namespace DataSummitModels.Models
{
    public class DSPage
    {
        public string BlobUrl { get; set; }
        public Enums.Document.Type Type { get; set; } = Enums.Document.Type.Unknown;
        public double TypeConfidence { get; set; } = 0;

        public DSPage()
        { ; }
    }
}
