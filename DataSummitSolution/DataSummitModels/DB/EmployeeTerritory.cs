﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataSummitModels.DB
{
    public partial class EmployeeTerritory
    {
        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
    }
}
