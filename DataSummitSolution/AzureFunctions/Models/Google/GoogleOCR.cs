using AzureFunctions.Models.Google.Response;
using System;

namespace AzureFunctions.Models.Google
{
    [Serializable]
    public class GoogleOCR
    {
        public String FileName { get; set; }
        public Uri Uri { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Cloud Blobs { get; set; }

        public GoogleOCR()
        {
            //Blobs = new List<Cloud>();
        }
    }
}
