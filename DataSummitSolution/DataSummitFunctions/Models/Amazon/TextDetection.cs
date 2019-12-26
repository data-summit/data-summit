using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class TextDetection
    {
        public float Confidence { get; set; }
        public string DetectedText { get; set; }
        public Geometry Geometry { get; set; }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public Type Type { get; set; }

        public TextDetection()
        { }
    }
}
