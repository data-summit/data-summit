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

        public List<TemplateAttributes> GetAllTemplateVersionTemplateAttributes(int templateVersionId)
        {
            List<TemplateAttributes> templateattributes = new List<TemplateAttributes>();
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

        public TemplateAttributes GetTemplateAttributesById(int templateAttributeId)
        {
            TemplateAttributes templateattributes = new TemplateAttributes();
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

        public long CreateTemplateAttribute(TemplateAttributes templateAttribute)
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
        public void UpdateTemplateAttribute(long id, TemplateAttributes templateAttribute)
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
                TemplateAttributes templateattribute = dataSummitDbContext.TemplateAttributes.First(p => p.TemplateAttributeId == templateAttributeId);
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
