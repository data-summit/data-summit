using System;
using DsDb = DataSummitModels.DB;

namespace DataSummitHelper.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DrawingPropertyDto
    {
        public int Id { get; set; }
        public string StandardName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public decimal? Confidence { get; set; }

        public DrawingPropertyDto(DsDb.ProfileAttributes profileAttribute)
        {
            Id = profileAttribute.ProfileVersionId.Value;
            StandardName = profileAttribute.StandardAttribute.Name;
            Name = profileAttribute.Name;
            Value = profileAttribute.Value;
            Confidence = null;
        }
    }
}
