using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.DTO
{
    public class ProfileVersionDTO : DataSummitModels.ProfileVersions
    {
        public string ImageString { get; set; }

        public ProfileVersionDTO()
        { }
    }
}
