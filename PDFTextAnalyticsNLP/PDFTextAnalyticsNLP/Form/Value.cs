using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Value
    {
        public string text { get; set; }
        public List<double> boundingBox { get; set; }
        public double confidence { get; set; }

        public Value()
        {
            boundingBox = new List<double>();
        }
    }
}
