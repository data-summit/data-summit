using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class PaperOrientations
    {
        public PaperOrientations()
        {
            Drawings = new HashSet<Drawings>();
        }

        public byte PaperOrientationId { get; set; }
        public string Orientation { get; set; }

        public virtual ICollection<Drawings> Drawings { get; set; }
    }
}
