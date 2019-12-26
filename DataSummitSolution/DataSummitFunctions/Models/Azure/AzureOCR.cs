using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Azure
{
    public class AzureOCR
    {
        public String Language { get; set; }
        public String Orientation { get; set; }
        public List<Region> Regions { get; set; }
        public Double? TextAngle { get; set; }

        public AzureOCR()
        {
            Regions = new List<Region>();
        }
    }
}
