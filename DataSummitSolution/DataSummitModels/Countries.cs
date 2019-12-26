using System;
using System.Collections.Generic;

namespace DataSummitModels
{
    public partial class Countries
    {
        public Countries()
        {
            Addresses = new HashSet<Addresses>();
        }

        public short CountryId { get; set; }
        public string Iso { get; set; }
        public string Name { get; set; }
        public string SentenceCaseName { get; set; }
        public string Iso3 { get; set; }
        public short? Numcode { get; set; }
        public int Phonecode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<Addresses> Addresses { get; set; }
    }
}
