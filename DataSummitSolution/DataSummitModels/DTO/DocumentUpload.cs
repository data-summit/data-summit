using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DataSummitModels.DTO
{
    public class DocumentUpload
    {
        public DocumentContentType DocumentType { get; set; } = DocumentContentType.Unknown;
        public DocumentExtension DocumentFormat { get; set; } = DocumentExtension.Unknown;
        public Enums.Payment.Plan PaymentPlan { get; set; } = Enums.Payment.Plan.Free;
        public int CompanyId { get; set; } = 0;
        public string UserId { get; set; } = "0";
        public string BlobUrl { get; set; } = "";
        public string ContainerName { get; set; } = Guid.NewGuid().ToString();
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public IFormFile File { get; set; }
        public virtual List<DB.FunctionTask> Tasks { get; set; }
        public bool IsUploaded { get; set; } = false;
        public int ProjectId { get; set; }
        public int TemplateId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }


        public DocumentUpload()
        { ; }
    }
}
