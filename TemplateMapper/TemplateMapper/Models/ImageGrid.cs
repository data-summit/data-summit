using System;
using System.Collections.Generic;
using System.Drawing;
using TemplateMapper.OCR.Consolidated;

namespace Models
{
    public class ImageGrid
    {
        public long ImageGridId { get; set; }
        public string Name { get; set; }
        public int WidthStart { get; set; }
        public int HeightStart { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string BlobUrl { get; set; }
        public bool Processed { get; set; }
        public ImageType Type { get; set; }
        public Image Image { get; set; }
        public long DrawingId { get; set; }
        public List<Sentence> Sentences { get; set; }

        public virtual Drawing Drawing { get; set; }

        public enum ImageType
        {
            Normal = 1,
            Overlap = 2
        }

        public ImageGrid()
        {
            WidthStart = 0;
            HeightStart = 0;
            Width = 0;
            Height = 0;
            Processed = false;
        }
    }
}