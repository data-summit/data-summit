using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public class DocumentUpload
    {
        public Document.Type DocumentType { get; set; } = Document.Type.Unknown;
        public Document.Format DocumentFormat { get; set; } = Document.Format.Unknown;
        public Payment.Plan PaymentPlan { get; set; } = Payment.Plan.Free;
        public int CompanyId { get; set; } = 0;
        public string UserId { get; set; } = "0";
        public string BlobUrl { get; set; } = "";
        public string ContainerName { get; set; } = Guid.NewGuid().ToString();
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public IFormFile File { get; set; }
        public virtual List<Tasks> Tasks { get; set; }
        public bool IsUploaded { get; set; } = false;

        public DocumentUpload()
        { ; }
    }
}
