using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Header
    {
        public string text { get; set; }
        public List<double> boundingBox { get; set; }

        public Header()
        {
            boundingBox = new List<double>();
        }
    }
}
