using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class Pages
    {
        public Property property { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<Blocks> blocks = new List<Blocks>();

        public Pages()
        { }
    }
}
