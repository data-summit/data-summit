using DataSummitFunctions.Models.Consolidated;
using DataSummitFunctions.Models.Google.Request;
using DataSummitFunctions.Models.Google.Response;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DataSummitFunctions.Models.Google
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
