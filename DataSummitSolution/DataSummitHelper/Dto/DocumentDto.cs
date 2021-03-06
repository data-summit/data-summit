using System;
using DataSummitModels.DB;

namespace DataSummitService.Dto
{
    /// <summary>
    /// </summary>
    public sealed class DocumentDto
    {
        public long DocumentId { get; set; }
        public string Name { get; set; }
        public string ContainerUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DocumentDto(Document document)
        {
            DocumentId = document.DocumentId;
            Name = document.FileName;
            ContainerUrl = document.ContainerName;
            CreatedDate = document.CreatedDate;
        }
    }
}
