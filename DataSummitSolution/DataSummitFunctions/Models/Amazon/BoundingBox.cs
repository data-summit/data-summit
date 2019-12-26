using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class BoundingBox
    {
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }

        public BoundingBox()
        { }

        public BoundingBox(double height, double left, double top, double width)
        {
            Height = height;
            Left = left;
            Top = top;
            Width = width;
        }
    }
}
