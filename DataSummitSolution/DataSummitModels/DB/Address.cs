using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class Address
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

        public virtual Company Company { get; set; }
        public virtual Country Country { get; set; }
        public virtual Project Project { get; set; }
    }
}
