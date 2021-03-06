using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Google.Response
{
    [Serializable]
    public class Paragraph
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Word> words { get; set; }

        public Paragraph()
        { }
    }
}
