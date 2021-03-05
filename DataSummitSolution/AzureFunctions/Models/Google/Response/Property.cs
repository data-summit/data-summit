using System;
using System.Collections.Generic;

namespace AzureFunctions.Models.Google.Response
{
    [Serializable]
    public class Property
    {
        public List<DetectedLanguage> detectedLanguages { get; set; }

        public Property()
        { }
    }
}
