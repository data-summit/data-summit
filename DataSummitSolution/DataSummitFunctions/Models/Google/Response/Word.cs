using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Word
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Symbol> symbols { get; set; }

        public Word()
        { }
    }
}
