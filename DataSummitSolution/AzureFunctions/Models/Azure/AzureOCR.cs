using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Azure
{
    public class AzureOCR
    {
        public String Language { get; set; }
        public String Orientation { get; set; }
        public List<Region> Regions { get; set; }
        public Double? TextAngle { get; set; }

        public AzureOCR()
        {
            Regions = new List<Region>();
        }
    }
}
