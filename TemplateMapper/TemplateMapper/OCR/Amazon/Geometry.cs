using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Amazon
{
    [Serializable]
    public class Geometry
    {
        public BoundingBox BoundingBox { get; set; }
        public List<Point> Polygon { get; set; }

        public Geometry()
        { }
    }
}
