using DataSummitHelper.Dto;
using System;

namespace DataSummitWeb.Models
{
    public class DocumentPropertyValue
    {
        public string Id { get; set; }
        public string Value { get; set; }

        public DocumentPropertyDto ToDto() => new DocumentPropertyDto()
        {
            SentenceId = Guid.Parse(Id),
            WordValue = Value
        };
    }
}
