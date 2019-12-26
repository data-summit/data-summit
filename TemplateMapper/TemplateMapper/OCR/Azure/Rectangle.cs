using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateMapper.OCR.Azure
{
    [Serializable]
    public class Rectangle
    {
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }

        public Rectangle()
        { }

        public Rectangle(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public static Rectangle CastTo(string rectangle)
        {
            Rectangle r = new Rectangle();
            List<string> lRes = rectangle.Split(',').ToList();
            int Left; int Top; int Width; int Height;
            int.TryParse(lRes[0], out Left);
            int.TryParse(lRes[1], out Top);
            int.TryParse(lRes[2], out Width);
            int.TryParse(lRes[3], out Height);
            r.Left = Left;
            r.Top = Top;
            r.Width = Width;
            r.Height = Height;
            return r;
        }
    }
}
