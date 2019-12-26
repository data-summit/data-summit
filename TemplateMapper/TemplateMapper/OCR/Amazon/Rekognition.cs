using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Amazon
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
