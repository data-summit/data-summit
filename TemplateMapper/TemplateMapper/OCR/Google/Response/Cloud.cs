using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    public class Cloud
    {
        public List<Responses> responses { get; set; }

        public Cloud()
        {
            responses = new List<Responses>();
        }
    }
}
