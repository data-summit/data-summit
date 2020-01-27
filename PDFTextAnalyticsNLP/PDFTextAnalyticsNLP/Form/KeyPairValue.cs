using System;
using System.Collections.Generic;
using System.Text;

namespace PDFTextAnalyticsNLP.Form
{
    public class KeyValuePair
    {
        public List<Key> key { get; set; }
        public List<Value> value { get; set; }

        public KeyValuePair()
        {
            key = new List<Key>();
            value = new List<Value>();
        }
    }
}
