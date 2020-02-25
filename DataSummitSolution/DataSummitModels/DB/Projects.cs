using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class Projects
    {
        public Projects()
        {
            Addresses = new HashSet<Addresses>();
            Drawings = new HashSet<Drawings>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string BlobStorageName { get; set; }
        public string BlobStorageKey { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UserId { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<Addresses> Addresses { get; set; }
        public virtual ICollection<Drawings> Drawings { get; set; }
    }
}
