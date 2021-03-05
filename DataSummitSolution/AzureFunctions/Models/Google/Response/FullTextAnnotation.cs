using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Google.Response
{
    [Serializable]
    public class FullTextAnnotation
    {
        public List<Page> pages { get; set; }
        public String text { get; set; }

        public FullTextAnnotation()
        { }
    }
}
