using System;
using System.Collections.Generic;

namespace DataSummitModels.DB
{
    public partial class EmployeeTerritories
    {
        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
    }
}
