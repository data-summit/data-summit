using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class AzureCompanyResourceUrl
    {
        public long AzureCompanyResourceUrlId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Key { get; set; }
        public string ResourceType { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
