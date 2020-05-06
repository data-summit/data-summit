using DataSummitModels.DB;
using System;

namespace DataSummitHelper.Dto
{
    public class TemplateAttributeDto
    {
        public long TemplateAttributeId { get; set; }
        public string StandardAttributeName { get; set; }
        public string Name { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public short? Width { get; set; }
        public short? Height { get; set; }
        public DateTime? CreatedDate { get; set; }

        public TemplateAttributeDto(ProfileAttributes profileAttribute)
        {
            TemplateAttributeId = profileAttribute.ProfileAttributeId;
            StandardAttributeName = profileAttribute.StandardAttribute.Name;
            Name = profileAttribute.Name;
            X = profileAttribute.ValueX;
            Y = profileAttribute.ValueY;
            Width = profileAttribute.ValueWidth;
            Height = profileAttribute.ValueHeight;
            CreatedDate = profileAttribute.CreatedDate;
        }
    }
}