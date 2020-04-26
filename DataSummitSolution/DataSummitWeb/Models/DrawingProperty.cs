using DataSummitHelper.Dto;

namespace DataSummitWeb.Models
{
    public class DrawingProperty
    {
        public int Id { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public decimal? Confidence { get; set; }

        public static DrawingProperty FromDto(DrawingPropertyDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new DrawingProperty
            {
                Id = dto.Id,
                StandardName = dto.StandardName,
                Name = dto.Name,
                Value = dto.Value,
                Confidence = dto.Confidence
            };
        }
    }
}
