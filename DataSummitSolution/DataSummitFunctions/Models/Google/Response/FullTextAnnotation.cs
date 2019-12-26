using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class FullTextAnnotation
    {
        public List<Page> pages { get; set; }
        public String text { get; set; }

        public FullTextAnnotation()
        { }
    }
}
