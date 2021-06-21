using System.Collections.Generic;

namespace DataSummitModels.Cloud.CognitiveServices.OCR
{
    public class Lines
    {
        public string BoundingBox { get; set; }
        public IList<Words> Words { get; set; }

        public Lines()
        { ; }
    }
}
