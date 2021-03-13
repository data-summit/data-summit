using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Project
    {
        public Project()
        {
            Addresses = new HashSet<Address>();
            Documents = new HashSet<Document>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UserId { get; set; }
        public string Region { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
