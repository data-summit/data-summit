using DataSummitFunctions.Models;

namespace DataSummitFunctions
{
    public partial class DrawingFeature
    {
        public long DrawingFeatureId { get; set; }
        public string Value { get; set; }
        public long Left { get; set; }
        public long Top { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public long Center { get; set; }
        public double Rotation { get; set; }
        public FeatureType Feature { get; set; }
        public long DrawingId { get; set; }

        public virtual Drawing Drawing { get; set; }

        public DrawingFeature()
        { }

        public enum FeatureType
        {
            Text = 1,
            Symbol = 2
        }
    }
}
