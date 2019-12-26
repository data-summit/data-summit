using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Request
{
    public class ImageAndFeat
    {
        public Image image { get; set; }
        public List<Features> features = new List<Features>();

        public ImageAndFeat()
        {
            image = new Image();
            features.Add(new Features("TEXT_DETECTION"));
        }
    }
}
