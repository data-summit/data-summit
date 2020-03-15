using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Page
    {
        public int number { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int clusterId { get; set; }
        public List<KeyValuePair> keyValuePairs { get; set; }
        public List<Table> tables { get; set; }

        public Page()
        {
            keyValuePairs = new List<KeyValuePair>();
            tables = new List<Table>();
        }
    }
}
