using System;

namespace AzureFunctions.Models.Amazon
{
    [Serializable]
    public class Polygon
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Polygon()
        { }
    }
}
