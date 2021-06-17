using System.Collections.Generic;

namespace DataSummitModels.Cloud.CognitiveServices.OCR
{
    public class Regions
    {
        public string BoundingBox { get; set; }
        public IList<Lines> Lines { get; set; }

        public Regions()
        { ; }
    }
}
