using System;
using System.Collections.Generic;


namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Responses
    {
        public List<TextAnnotation> textAnnotations { get; set;} 
        public FullTextAnnotation fullTextAnnotation { get; set; }

        public Responses()
        { }
    }
}
