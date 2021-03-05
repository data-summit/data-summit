using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureFunctions.Models.Consolidated
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
            Sentences s = new Sentences
            {
                SentenceId = Guid.NewGuid(),
                IsUsed = this.IsUsed,
                Left = this.Left,
                Top = this.Top,
                Width = this.Width,
                Height = this.Height,
                Vendor = this.Vendor,
                Words = this.Words,
                SlendernessRatio = this.SlendernessRatio
            };
            return s;
        }
        public Models.Sentences ToModel()
        {
            Models.Sentences m = new Models.Sentences();
            m.Confidence = Confidence;
            m.Height = Height;
            m.IsUsed = IsUsed;
            m.Left = Left;
            m.Properties = Properties.Select(p => p.ToModel()).ToList();
            m.SentenceId = SentenceId;
            m.SlendernessRatio = SlendernessRatio;
            m.Top = Top;
            m.Vendor = Vendor;
            m.Width = Width;
            m.Words = Words;

            return m;
        }
    }
}
