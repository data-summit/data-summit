using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Paragraph
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Word> words { get; set; }

        public Paragraph()
        { }
    }
}
