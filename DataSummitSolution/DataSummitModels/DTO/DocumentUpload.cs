using DataSummitModels.DB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public class DocumentUpload
    {
        public Enums.Document.Type DocumentType { get; set; } = Enums.Document.Type.Unknown;
        public Enums.Document.Format DocumentFormat { get; set; } = Enums.Document.Format.Unknown;
        public Enums.Payment.Plan PaymentPlan { get; set; } = Enums.Payment.Plan.Free;
        public int CompanyId { get; set; } = 0;
        public string UserId { get; set; } = "0";
        public string BlobUrl { get; set; } = "";
        public string ContainerName { get; set; } = Guid.NewGuid().ToString();
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public IFormFile File { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public bool IsUploaded { get; set; } = false;
        public int ProjectId { get; set; }
        public int TemplateId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }


        public DocumentUpload()
        { ; }
    }
}
