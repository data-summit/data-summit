﻿using System.Collections.Generic;
using System.Drawing;

namespace DataSummitModels.DB
{
    public partial class ImageGrids
    {
        public long ImageGridId { get; set; }
        public string Name { get; set; }
        public int WidthStart { get; set; }
        public int HeightStart { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string BlobUrl { get; set; }
        public byte IterationAmazon { get; set; }
        public byte IterationAzure { get; set; }
        public byte IterationGoogle { get; set; }
        public bool ProcessedAmazon { get; set; }
        public bool ProcessedAzure { get; set; }
        public bool ProcessedGoogle { get; set; }
        public Enums.Image.Type Type { get; set; }
        public long DrawingId { get; set; }
        public Image Image { get; set; }
        public List<Sentences> Sentences { get; set; }

        public virtual Drawings Drawing { get; set; }
    }
}