using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class Addresses
    {
        public long AddressId { get; set; }
        public string NumberName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string TownCity { get; set; }
        public string County { get; set; }
        public short CountryId { get; set; }
        public string PostCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CompanyId { get; set; }
        public int? ProjectId { get; set; }

        public virtual Companies Company { get; set; }
        public virtual Countries Country { get; set; }
        public virtual Projects Project { get; set; }
    }
}
