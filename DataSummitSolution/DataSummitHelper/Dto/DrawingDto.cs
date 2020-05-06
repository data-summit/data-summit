using System;
using DataSummitModels.DB;

namespace DataSummitHelper.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DrawingDto
    {
        public long DrawingId { get; set; }
        public string Name { get; set; }
        public string ContainerUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DrawingDto(Drawings drawing)
        {
            DrawingId = drawing.DrawingId;
            Name = drawing.FileName;
            ContainerUrl = drawing.ContainerName;
            CreatedDate = drawing.CreatedDate;
        }
    }
}
