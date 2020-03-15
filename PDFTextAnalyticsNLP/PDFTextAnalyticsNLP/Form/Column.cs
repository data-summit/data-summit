using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Column
    {
        public List<Header> header { get; set; }
        public List<object> entries { get; set; }

        public Column()
        {
            header = new List<Header>();
            entries = new List<object>();
        }
    }
}
