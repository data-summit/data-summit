using DataSummitService.Classes;
using System;

namespace DataSummitService.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DocumentPropertyDto
    {
        public long TemplateAttributeId { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public decimal? Confidence { get; set; }
        public Guid SentenceId { get; set; }
        public string WordValue { get; set; }

        public DocumentPropertyDto()
        { ; }

        public DocumentPropertyDto(Classes.DocumentPropertyDto documentProperty)
        {
            TemplateAttributeId = documentProperty.TemplateAttributes.TemplateAttributeId;
            StandardName = documentProperty.TemplateAttributes.StandardAttribute.Name;
            Name = documentProperty.TemplateAttributes.Name;
            Confidence = null;
            SentenceId = documentProperty.Sentences.SentenceId;
            WordValue = documentProperty.Sentences.Words;
        }
    }
}
