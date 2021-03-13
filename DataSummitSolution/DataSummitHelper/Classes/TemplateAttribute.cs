using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class TemplateAttribute
    {
        private DataSummitDbContext dataSummitDbContext;

        public TemplateAttribute(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.TemplateAttribute> GetAllTemplateVersionTemplateAttributes(int templateVersionId)
        {
            List<DataSummitModels.DB.TemplateAttribute> templateattributes = new List<DataSummitModels.DB.TemplateAttribute>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                templateattributes = dataSummitDbContext.TemplateAttributes.Where(e => e.TemplateVersionId == templateVersionId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return templateattributes;
        }

        public DataSummitModels.DB.TemplateAttribute GetTemplateAttributesById(int templateAttributeId)
        {
            DataSummitModels.DB.TemplateAttribute templateattributes = new DataSummitModels.DB.TemplateAttribute();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                templateattributes = dataSummitDbContext.TemplateAttributes.First(e => e.TemplateAttributeId == templateAttributeId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return templateattributes;
        }

        public long CreateTemplateAttribute(DataSummitModels.DB.TemplateAttribute templateAttribute)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                templateAttribute.CreatedDate = DateTime.Now;
                //templateattribute.UserId = OAuthServer.User.Identity.Name;
                dataSummitDbContext.TemplateAttributes.Add(templateAttribute);
                dataSummitDbContext.SaveChanges();
                returnid = templateAttribute.TemplateAttributeId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }
        public void UpdateTemplateAttribute(long id, DataSummitModels.DB.TemplateAttribute templateAttribute)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.TemplateAttributes.Update(templateAttribute);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteTemplateAttribute(long templateAttributeId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.TemplateAttribute templateattribute = dataSummitDbContext.TemplateAttributes.First(p => p.TemplateAttributeId == templateAttributeId);
                dataSummitDbContext.TemplateAttributes.Remove(templateattribute);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
