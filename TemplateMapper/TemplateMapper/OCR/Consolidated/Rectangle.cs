using System;
using System.Collections.Generic;

namespace TemplateMapper.OCR.Consolidated
{
    public class Rectangle
    {
        public long Height { get; set; }
        public long Left { get; set; }
        public long Top { get; set; }
        public long Width { get; set; }

        public Rectangle()
        { }

        public Rectangle(long left, long top, long width, long height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public Boolean IsAlmostEqualTo(Rectangle rTest, long Tolerance = 5)
        {
            Boolean bTop = false; Boolean bLeft = false; Boolean bWidth = false; Boolean bHeight = false;
            if (this.Equals(rTest) == false)
            {
                if (Math.Abs(this.Top - rTest.Top) <= Tolerance)
                { bTop = true; }
                if (Math.Abs(this.Left - rTest.Left) <= Tolerance)
                { bLeft = true; }
                if (Math.Abs((this.Left + this.Width) - (rTest.Left + rTest.Width)) <= Tolerance)
                { bWidth = true; }
                if (Math.Abs((this.Top + this.Height) - (rTest.Top + rTest.Height)) <= Tolerance)
                { bHeight = true; }
            }
            else
            { return true; }
            if(bTop == true && bLeft == true && bWidth == true && bHeight == true)
            { return true; }
            return false;
        }
        public System.Drawing.Rectangle ToRect()
        {
            return new System.Drawing.Rectangle(
                        (int)this.Left, (int)this.Top,
                        (int)this.Width, (int)this.Height);
        }
        public List<System.Drawing.Point> ToPoints()
        {
            List<System.Drawing.Point> lPt = new List<System.Drawing.Point>();
            lPt.Add(new System.Drawing.Point((int)this.Left, (int)this.Top));
            lPt.Add(new System.Drawing.Point((int)(this.Left + this.Width), (int)this.Top));
            lPt.Add(new System.Drawing.Point((int)this.Left, (int)(this.Top + this.Height)));
            lPt.Add(new System.Drawing.Point((int)(this.Left + this.Width), (int)(this.Top + this.Height)));
            return lPt;
        }
    }
}
