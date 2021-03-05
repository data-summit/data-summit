using System.Collections.Generic;

namespace AzureFunctions.Models
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
