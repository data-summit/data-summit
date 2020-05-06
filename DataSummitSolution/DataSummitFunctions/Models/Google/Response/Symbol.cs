using System;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Symbol
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public string text { get; set; }

        public Symbol()
        { }
    }
}
