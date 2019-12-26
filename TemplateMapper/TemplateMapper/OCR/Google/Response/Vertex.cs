using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }

        public Vertex()
        { }
    }
}
