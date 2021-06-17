using System.Collections.Generic;

namespace DataSummitModels.Cloud.CognitiveServices.OCR
{
    public class Application
    {
        public string Language { get; set; }
        public double TextAngle { get; set; }
        public string Orientation { get; set; }
        public IList<Regions> Regions { get; set; }

        public Application()
        { ; }
    }
}
