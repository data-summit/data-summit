using DataSummitHelper.Classes;
using System;

namespace DataSummitHelper.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DocumentPropertyDto
    {
        public long ProfileAttributeId { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public decimal? Confidence { get; set; }
        public Guid SentenceId { get; set; }
        public string WordValue { get; set; }

        public DocumentPropertyDto()
        { ; }

        public DocumentPropertyDto(DocumentProperty documentProperty)
        {
            ProfileAttributeId = documentProperty.ProfileAttributes.ProfileAttributeId;
            StandardName = documentProperty.ProfileAttributes.StandardAttribute.Name;
            Name = documentProperty.ProfileAttributes.Name;
            Confidence = null;
            SentenceId = documentProperty.Sentences.SentenceId;
            WordValue = documentProperty.Sentences.Words;
        }
    }
}
