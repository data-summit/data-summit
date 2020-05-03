using System;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class TextAnnotation
    {
        public String locale { get; set; }
        public String description { get; set; }
        public BoundingPoly boundingPoly { get; set; }

        public TextAnnotation()
        { }
    }
}
