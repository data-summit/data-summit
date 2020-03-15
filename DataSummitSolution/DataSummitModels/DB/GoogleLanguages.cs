using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class GoogleLanguages
    {
        public byte GoogleLanguageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
    }
}
