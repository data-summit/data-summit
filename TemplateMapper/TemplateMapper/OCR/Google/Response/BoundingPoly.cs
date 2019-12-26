using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class BoundingPoly
    {
        public List<Vertex> vertices { get; set; }

        public BoundingPoly()
        { }
    }
}
