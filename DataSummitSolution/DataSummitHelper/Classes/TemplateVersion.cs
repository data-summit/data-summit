using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class TemplateVersion
    {
        private DataSummitDbContext dataSummitDbContext;

        public TemplateVersion(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.TemplateVersion> GetAllCompanyTemplateVersions(int companyId)
        {
            List<DataSummitModels.DB.TemplateVersion> templateversions = new List<DataSummitModels.DB.TemplateVersion>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                templateversions = dataSummitDbContext.TemplateVersions.Where(e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return templateversions;
        }

        public DataSummitModels.DB.TemplateVersion GetTemplateVersion(int templateVersionId)
        {
            DataSummitModels.DB.TemplateVersion templateVersion = new DataSummitModels.DB.TemplateVersion();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();

                templateVersion = dataSummitDbContext.TemplateVersions
                                    .FirstOrDefault(e => e.TemplateVersionId == templateVersionId);
                templateVersion.TemplateAttributes = dataSummitDbContext.TemplateAttributes
                                    .Where(e => e.TemplateVersionId == templateVersion.TemplateVersionId).ToList();

                foreach (DataSummitModels.DB.TemplateAttribute pa in templateVersion.TemplateAttributes)
                {
                    pa.StandardAttribute = dataSummitDbContext.StandardAttributes
                                            .FirstOrDefault(e => e.StandardAttributeId == pa.StandardAttributeId);
                    pa.TemplateVersion = null;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return templateVersion;
        }

        public int CreateTemplateVersion(DataSummitModels.DB.TemplateVersion templateVersion)
        {
            int returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.TemplateVersions.Add(templateVersion);
                dataSummitDbContext.SaveChanges();
                returnid = templateVersion.TemplateVersionId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return returnid;
        }
        public void UpdateTemplateVersion(int id, DataSummitModels.DB.TemplateVersion templateVersion)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.TemplateVersions.Update(templateVersion);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteTemplateVersion(int templateVersionId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.TemplateVersion templateversion = dataSummitDbContext.TemplateVersions.First(p => p.TemplateVersionId == templateVersionId);
                dataSummitDbContext.TemplateVersions.Remove(templateversion);
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
