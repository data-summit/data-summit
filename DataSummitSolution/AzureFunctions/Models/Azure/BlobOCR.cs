using System;

namespace AzureFunctions.Models.Azure
{
    [Serializable]
    public class BlobOCR
    {
        public String FileName { get; set; }
        public Uri Uri { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public AzureOCR Results { get; set; }

        public BlobOCR()
        {
            Results = new AzureOCR();
        }
    }
}
