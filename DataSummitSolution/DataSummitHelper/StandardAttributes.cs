using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class StandardAttributes
    {
        private DataSummitDbContext dataSummitDbContext;

        public StandardAttributes(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.StandardAttributes> GetAllCompanyStandardAttributes(int id)
        {
            List<DataSummitModels.DB.StandardAttributes> lStandardAttributes = new List<DataSummitModels.DB.StandardAttributes>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                lStandardAttributes = dataSummitDbContext.StandardAttributes.Where(c => 
                        c.ProfileAttributes.First().ProfileAttributeId == id).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lStandardAttributes;
        }

        public DataSummitModels.DB.StandardAttributes GetStandardAttributesById(short standardAttributeId)
        {
            DataSummitModels.DB.StandardAttributes StandardAttributes = new DataSummitModels.DB.StandardAttributes();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                StandardAttributes = dataSummitDbContext.StandardAttributes.First(p => p.StandardAttributeId == standardAttributeId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return StandardAttributes;
        }

        public long CreateStandardAttribute(DataSummitModels.DB.StandardAttributes standardAttribute)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.StandardAttributes.Add(standardAttribute);
                dataSummitDbContext.SaveChanges();
                returnid = standardAttribute.StandardAttributeId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdateStandardAttribute(short id, DataSummitModels.DB.StandardAttributes standardAttribute)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.StandardAttributes.Update(standardAttribute);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteStandardAttribute(short standardAttributeId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.StandardAttributes StandardAttributes = dataSummitDbContext.StandardAttributes.First(p => p.StandardAttributeId == standardAttributeId);
                dataSummitDbContext.StandardAttributes.Remove(StandardAttributes);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
