using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Property
    {
        DataSummitDbContext dataSummitDbContext;

        public Property(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public long CreateProperty(DataSummitModels.DB.Property property)
        {
            long returnlong = long.MinValue;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Properties.Add(property);
                dataSummitDbContext.SaveChanges();
                returnlong = property.PropertyId;
            }
            catch (Exception ae)
            { }
            return returnlong;
        }
    }
}
