using DataSummitHelper.Classes;
using System;

namespace DataSummitHelper.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DrawingPropertyDto
    {
        public long ProfileAttributeId { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public decimal? Confidence { get; set; }
        public Guid SentenceId { get; set; }
        public string WordValue { get; set; }

        public DrawingPropertyDto(DrawingProperty drawingProperty)
        {
            ProfileAttributeId = drawingProperty.ProfileAttributes.ProfileAttributeId;
            StandardName = drawingProperty.ProfileAttributes.StandardAttribute.Name;
            Name = drawingProperty.ProfileAttributes.Name;
            Confidence = null;
            SentenceId = drawingProperty.Sentences.SentenceId;
            WordValue = drawingProperty.Sentences.Words;
        }
    }
}
