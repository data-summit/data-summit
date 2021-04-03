using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class PaperOrientation
    {
        public PaperOrientation()
        {
            Documents = new HashSet<Document>();
        }

        public byte PaperOrientationId { get; set; }
        public string Orientation { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
