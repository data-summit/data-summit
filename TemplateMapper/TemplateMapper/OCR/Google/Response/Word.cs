using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class Word
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Symbol> symbols { get; set; }

        public Word()
        { }
    }
}
