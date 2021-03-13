using DataSummitHelper.Dto;
using System;

namespace DataSummitWeb.Models
{
    public class DocumentProperty
    {
        public long ProfileAttributeId { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public decimal? Confidence { get; set; }
        public Guid SentenceId { get; set; }
        public string WordValue { get; set; }

        public static DocumentProperty FromDto(DocumentPropertyDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new DocumentProperty
            {
                ProfileAttributeId = dto.ProfileAttributeId,
                StandardName = dto.StandardName,
                Name = dto.Name,
                Confidence = null,
                SentenceId = dto.SentenceId,
                WordValue = dto.WordValue
        };
        }
    }
}
