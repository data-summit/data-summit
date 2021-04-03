using DataSummitModels.BHoM.Geometry;
using System;

namespace DataSummitModels.Cloud
{
    public class MLPrediction
    {
        public double Probability { get; set; }
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public string TagType { get; set; }

        public MLPrediction()
        { ; }

        public MLPrediction(double probability = 0, Guid tagId = default, string tagName = null, BoundingBox boundingBox = null, string tagType = null)
        {
            Probability = probability;
            TagId = tagId;
            TagName = tagName;
            BoundingBox = boundingBox;
            TagType = tagType;
        }
    }
}
