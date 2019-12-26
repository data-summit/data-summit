using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class Currencies
    {
        public short CurrencyId { get; set; }
        public string Entity { get; set; }
        public string Name { get; set; }
        public string AlphabeticCode { get; set; }
        public string NumericCode { get; set; }
        public string MinorUnit { get; set; }
        public bool IsFundYesNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
    }
}
