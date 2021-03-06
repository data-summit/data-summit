using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Azure
{
    [Serializable]
    public class Region
    {
        public string BoundingBox { get; set; }
        public List<Line> Lines = new List<Line>();
        public Rectangle Rectangle { get; set; }

        public Region()
        {
            Rectangle = new Rectangle();
        }

        public static Region CastTo(OcrRegion region)
        {
            Region r = new Region();
            r.BoundingBox = region.BoundingBox;
            r.Rectangle = Rectangle.CastTo(region.BoundingBox);
            foreach(OcrLine l in region.Lines)
            { r.Lines.Add(Line.CastTo(l)); }
            return r;
        }
    }
}
