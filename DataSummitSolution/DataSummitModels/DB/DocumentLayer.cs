using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class DocumentLayer
    {
        public long DocumentLayerId { get; set; }
        public string Name { get; set; }
        public long DocumentId { get; set; }

        public virtual Document Document { get; set; }
    }
}
