using System;
using System.Collections.Generic;
using DataSummitHelper.Dto;

namespace DataSummitWeb.Models
{
    public class Document
    {
        public long DocumentId { get; set; }
        public string Name { get; set; }
        public string ContainerUrl { get; set; }
        public HashSet<int> PropertieIds { get; set; }
        public DateTime? CreatedDate { get; set; }

        public static Document FromDto(DocumentDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Document
            {
                DocumentId = dto.DocumentId,
                Name = dto.Name,
                ContainerUrl = dto.ContainerUrl,
                CreatedDate = dto.CreatedDate
            };
        }
    }
}
