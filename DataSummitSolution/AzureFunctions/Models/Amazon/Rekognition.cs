using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Amazon
{
    [Serializable]
    public class Rekognition
    {
        public List<TextDetection> TextDetections { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; }
        public long ContentLength { get; set; }
        public int HttpStatusCode { get; set; }

        public Rekognition()
        { }
    }
}
