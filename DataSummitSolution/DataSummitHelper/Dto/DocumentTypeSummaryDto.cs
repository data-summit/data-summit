using DataSummitModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitService.Dto
{
    public class DocumentTypeSummaryDto
    {
        public string BlobUrl { get; set; }
        public DocumentContentType DocumentType { get; set; }
    }
}
