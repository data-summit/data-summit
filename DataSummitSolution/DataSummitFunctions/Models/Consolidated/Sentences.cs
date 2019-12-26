using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Consolidated
{
    public class Sentences : ICloneable
    {
        public Sentences()
        {
            SentenceId = Guid.NewGuid();
            Properties = new HashSet<Properties>();
        }

        public Guid SentenceId { get; set; }
        public string Words { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public string Vendor { get; set; }
        public bool IsUsed { get; set; }
        public decimal? Confidence { get; set; }
        public decimal SlendernessRatio { get; set; }
        public long DrawingId { get; set; }

        //public virtual Drawings Drawing { get; set; }
        public virtual ICollection<Properties> Properties { get; set; }

        public Sentences(string words)
        {
            SentenceId = Guid.NewGuid();
            Words = words;
        }

        public Sentences(string words, Rectangle rectangle)
        {
            SentenceId = Guid.NewGuid();
            Words = words;
            Left = (int)rectangle.Left;
            Top = (int)rectangle.Top;
            Width = (int)rectangle.Width;
            Height = (int)rectangle.Height;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public Sentences(string words, Rectangle rectangle, string vendor)
        {
            SentenceId = Guid.NewGuid();
            Words = words;
            Left = (int)rectangle.Left;
            Top = (int)rectangle.Top;
            Width = (int)rectangle.Width;
            Height = (int)rectangle.Height;
            Vendor = vendor;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public Sentences(string words, Rectangle rectangle, string vendor, bool isUsed)
        {
            SentenceId = Guid.NewGuid();
            Words = words;
            Left = (int)rectangle.Left;
            Top = (int)rectangle.Top;
            Width = (int)rectangle.Width;
            Height = (int)rectangle.Height;
            Vendor = vendor;
            IsUsed = IsUsed;
            SlendernessRatio = rectangle.Width / rectangle.Height;
        }

        public virtual object Clone()
        {
            Sentences s = new Sentences();
            s.SentenceId = Guid.NewGuid();
            s.IsUsed = this.IsUsed;
            s.Left = this.Left;
            s.Top = this.Top;
            s.Width = this.Width;
            s.Height = this.Height;
            s.Vendor = this.Vendor;
            s.Words = this.Words;
            s.SlendernessRatio = this.SlendernessRatio;
            return s;
        }
    }
}
