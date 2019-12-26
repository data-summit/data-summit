using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.DTO
{
    public class RectanglePairs
    {
        public int CompanyId { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public string TemplateName { get; set; }
        public List<RectanglePair> Pairs { get; set; }

        public RectanglePairs()
        {
            Pairs = new List<RectanglePair>();
        }
    }
}
