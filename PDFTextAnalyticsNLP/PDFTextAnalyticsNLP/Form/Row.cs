using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFTextAnalyticsNLP.Form
{
    public class Row
    {
        public string text { get; set; }
        public List<double> boundingBox { get; set; }
        public double confidence { get; set; }

        public Row()
        {
            boundingBox = new List<double>();
        }
    }
}
