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

        public TemplateAttributeDto(DataSummitModels.DB.TemplateAttribute templateAttribute)
        {
            TemplateAttributeId = templateAttribute.TemplateAttributeId;
            StandardAttributeName = templateAttribute.StandardAttribute.Name;
            Name = templateAttribute.Name;
            X = templateAttribute.ValueX;
            Y = templateAttribute.ValueY;
            Width = templateAttribute.ValueWidth;
            Height = templateAttribute.ValueHeight;
            CreatedDate = templateAttribute.CreatedDate;
        }
    }
}