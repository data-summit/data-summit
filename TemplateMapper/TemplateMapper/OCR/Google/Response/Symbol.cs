using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class Symbol
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public string text { get; set; }

        public Symbol()
        { }
    }
}
