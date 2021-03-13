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

        public List<TemplateVersions> GetAllCompanyTemplateVersions(int companyId)
        {
            List<TemplateVersions> templateversions = new List<TemplateVersions>();
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

        public TemplateVersions GetTemplateVersion(int templateVersionId)
        {
            TemplateVersions templateVersion = new TemplateVersions();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();

                templateVersion = dataSummitDbContext.TemplateVersions
                                    .FirstOrDefault(e => e.TemplateVersionId == templateVersionId);
                templateVersion.TemplateAttributes = dataSummitDbContext.TemplateAttributes
                                    .Where(e => e.TemplateVersionId == templateVersion.TemplateVersionId).ToList();

                foreach (TemplateAttributes pa in templateVersion.TemplateAttributes)
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

        public int CreateTemplateVersion(TemplateVersions templateVersion)
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
        public void UpdateTemplateVersion(int id, TemplateVersions templateVersion)
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
                TemplateVersions templateversion = dataSummitDbContext.TemplateVersions.First(p => p.TemplateVersionId == templateVersionId);
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
