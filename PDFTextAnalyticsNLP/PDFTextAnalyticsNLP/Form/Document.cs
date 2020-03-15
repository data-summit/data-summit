using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class Document
    {
        public string dataRef { get; set; }
        public List<Page> pages { get; set; }

        public Document()
        {
            pages = new List<Page>();
        }
    }
}
