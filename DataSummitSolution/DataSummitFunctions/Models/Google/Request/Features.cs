using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Google.Request
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
