using DataSummitHelper.Dto;
using System;

namespace DataSummitWeb.Models
{
    public class DrawingPropertyValue
    {
        public string Id { get; set; }
        public string Value { get; set; }

        public DrawingPropertyDto ToDto() => new DrawingPropertyDto()
        {
            SentenceId = Guid.Parse(Id),
            WordValue = Value
        };
    }
}
