using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class AzureCompanyResourceUrls
    {
        public long AzureCompanyResourceUrlId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Key { get; set; }
        public string ResourceType { get; set; }
        public int CompanyId { get; set; }

        public virtual Companies Company { get; set; }
    }
}
