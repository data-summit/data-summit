using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitService.Dto
{
    public class FileUploadSummaryDto
    {
        public string BlobUrl { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
