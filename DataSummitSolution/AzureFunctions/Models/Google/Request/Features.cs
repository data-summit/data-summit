using System;

namespace AzureFunctions.Models.Google.Request
{
    public class Features
    {
        public String type { get; set; }

        public Features(String featuretype)
        {
            type = featuretype;
        }
    }
}
