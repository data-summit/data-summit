using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Key
    {
        public string text { get; set; }
        public List<double> boundingBox { get; set; }

        public Key()
        {
            boundingBox = new List<double>();
        }
    }
}
