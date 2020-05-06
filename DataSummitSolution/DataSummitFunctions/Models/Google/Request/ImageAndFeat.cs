using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Request
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
