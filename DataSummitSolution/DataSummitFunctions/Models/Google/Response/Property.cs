using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Property
    {
        public List<DetectedLanguage> detectedLanguages { get; set; }

        public Property()
        { }
    }
}
