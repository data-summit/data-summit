using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Table
    {
        public string id { get; set; }
        public List<Column> columns { get; set; }

        public Table()
        {
            columns = new List<Column>();
        }
    }
}
