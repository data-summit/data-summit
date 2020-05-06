using System;
using System.Collections.Generic;
using DataSummitHelper.Dto;

namespace DataSummitWeb.Models
{
    public class Drawing
    {
        public long DrawingId { get; set; }
        public string Name { get; set; }
        public string ContainerUrl { get; set; }
        public HashSet<int> PropertieIds { get; set; }
        public DateTime? CreatedDate { get; set; }

        public static Drawing FromDto(DrawingDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Drawing
            {
                DrawingId = dto.DrawingId,
                Name = dto.Name,
                ContainerUrl = dto.ContainerUrl,
                CreatedDate = dto.CreatedDate
            };
        }
    }
}
