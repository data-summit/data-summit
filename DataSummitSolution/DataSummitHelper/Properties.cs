using DataSummitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Properties
    {
        DataSummitDbContext dataSummitDbContext;

        public Properties(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.Properties> GetAllDrawingProperties(int drawingId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.Properties> properties = new List<DataSummitModels.Properties>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                properties = dataSummitDbContext.Properties
                                .Where(e => e.Sentence.DrawingId == drawingId)
                                .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return properties;
        }

        public long CreateProperty(DataSummitModels.Properties property, bool IsProdEnvironment = false)
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
            {
                string strError = ae.Message.ToString();
            }
            return returnlong;
        }

        public void UpdateProperty(int id, DataSummitModels.Properties property, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Properties.Update(property);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteProperty(long propertyId, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.Properties property = dataSummitDbContext.Properties.First(
                                                            p => p.PropertyId == propertyId);
                dataSummitDbContext.Properties.Remove(property);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
