using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class StandardAttribute
    {
        private DataSummitDbContext dataSummitDbContext;

        public StandardAttribute(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.StandardAttribute> GetAllCompanyStandardAttributes(int id)
        {
            List<DataSummitModels.DB.StandardAttribute> lStandardAttributes = new List<DataSummitModels.DB.StandardAttribute>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                lStandardAttributes = dataSummitDbContext.StandardAttributes.Where(c => 
                        c.TemplateAttributes.First().TemplateAttributeId == id).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lStandardAttributes;
        }

        public DataSummitModels.DB.StandardAttribute GetStandardAttributesById(short standardAttributeId)
        {
            DataSummitModels.DB.StandardAttribute StandardAttributes = new DataSummitModels.DB.StandardAttribute();
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

        public long CreateStandardAttribute(DataSummitModels.DB.StandardAttribute standardAttribute)
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

        public void UpdateStandardAttribute(short id, DataSummitModels.DB.StandardAttribute standardAttribute)
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
                DataSummitModels.DB.StandardAttribute StandardAttributes = dataSummitDbContext.StandardAttributes.First(p => p.StandardAttributeId == standardAttributeId);
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
