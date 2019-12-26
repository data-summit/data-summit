using DataSummitFunctions.Models.Consolidated;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DataSummitFunctions.Models
{
    public class ImageGrid
    {
        public string Name { get; set; }
        public Image Image { get; set; }
        public int WidthStart { get; set; }
        public int HeightStart { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string BlobURL { get; set; }
        public byte IterationAmazon { get; set; }
        public byte IterationAzure { get; set; }
        public byte IterationGoogle { get; set; }
        public bool ProcessedAmazon { get; set; }
        public bool ProcessedAzure { get; set; }
        public bool ProcessedGoogle { get; set; }
        public ImageType Type { get; set; }
        public long DrawingId { get; set; }
        public List<Sentences> Sentences { get; set; }

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
            IterationAmazon = 0;
            IterationAzure = 0;
            IterationGoogle = 0;
            ProcessedAmazon = false;
            ProcessedAzure = false;
            ProcessedGoogle = false;
        }
    }
}
