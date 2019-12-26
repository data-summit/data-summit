using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class ResponseMetadata
    {
        public string RequestId { get; set; }
        public Metadata Metadata { get; set; }

        public ResponseMetadata()
        { }
    }
}
