using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateMapper
{
    public class RectPair
    {
        public Rectangle Rect1 { get; set; }
        public Rectangle Rect2 { get; set; }
        public Line Connection { get; set; }
        public DataGridViewRow Row { get; set; }

        public RectPair()
        { }
    }
}
