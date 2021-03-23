using System.Collections.Generic;

namespace DataSummitModels.Models
{
    public class DSDocument
    {
        public string Name { get; set; }
        public string blobUrl { get; set; }
        public Enums.Document.Format Format { get; set; } = Enums.Document.Format.Unknown;
        public List<DSPage> Pages { get; set; } = new List<DSPage>();

        public DSDocument()
        { ; }
    }
}
