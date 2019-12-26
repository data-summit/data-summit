﻿using System;
using System.Collections.Generic;

namespace DBModelsMigration.DBMigrations
{
    public partial class Categories
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
    }
}
