using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Consolidated
{
    public class Sentence : ICloneable
    {
        public String FileName { get; set; }
        public String Words { get; set; }
        public Rectangle Rectangle { get; set; }
        public String Vendor { get; set; }
        public bool IsUsed { get; set; }
        public double SlendernessRatio { get; set; }
        

        public Sentence()
        { }

        public Sentence(String words)
        {
            Words = words;
        }

        public Sentence(String words, Rectangle rectangle)
        {
            Words = words;
            Rectangle = rectangle;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public Sentence(String words, Rectangle rectangle, String vendor)
        {
            Words = words;
            Rectangle = rectangle;
            Vendor = vendor;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public Sentence(String words, Rectangle rectangle, String vendor, String filename)
        {
            Words = words;
            Rectangle = rectangle;
            Vendor = vendor;
            FileName = filename;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public virtual object Clone()
        {
            Sentence s = new Sentence();
            s.FileName = this.FileName;
            s.IsUsed = this.IsUsed;
            s.Rectangle = new Rectangle(this.Rectangle.Left, this.Rectangle.Top, this.Rectangle.Width, this.Rectangle.Height);
            s.Vendor = this.Vendor;
            s.Words = this.Words;
            s.SlendernessRatio = this.SlendernessRatio;
            return s;
        }
    }
}
