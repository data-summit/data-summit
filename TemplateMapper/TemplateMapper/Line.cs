using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper
{
    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Line()
        { }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
}
