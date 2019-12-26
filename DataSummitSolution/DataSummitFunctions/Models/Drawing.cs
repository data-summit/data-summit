using DataSummitFunctions.Models.Consolidated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models
{
    public class Drawing
    {
        public List<Sentences> Sentences { get; set; }

        public Drawing()
        {
            Sentences = new List<Sentences>();
        }
    }
}
