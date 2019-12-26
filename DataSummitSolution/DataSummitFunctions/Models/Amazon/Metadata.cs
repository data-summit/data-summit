using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Amazon
{
    [Serializable]
    public class Metadata
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Metadata()
        { }
    }
}
