using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class ProfileVersions
    {
        private DataSummitDbContext dataSummitDbContext;

        public ProfileVersions(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.ProfileVersions> GetAllCompanyProfileVersions(int companyId)
        {
            List<DataSummitModels.DB.ProfileVersions> profileversions = new List<DataSummitModels.DB.ProfileVersions>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                profileversions = dataSummitDbContext.ProfileVersions.Where(e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return profileversions;
        }

        public DataSummitModels.DB.ProfileVersions GetProfileVersion(int profileVersionId)
        {
            DataSummitModels.DB.ProfileVersions profileVersion = new DataSummitModels.DB.ProfileVersions();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();

                profileVersion = dataSummitDbContext.ProfileVersions
                                    .FirstOrDefault(e => e.ProfileVersionId == profileVersionId);
                profileVersion.ProfileAttributes = dataSummitDbContext.ProfileAttributes
                                    .Where(e => e.ProfileVersionId == profileVersion.ProfileVersionId).ToList();

                foreach (DataSummitModels.DB.ProfileAttributes pa in profileVersion.ProfileAttributes)
                {
                    pa.StandardAttribute = dataSummitDbContext.StandardAttributes
                                            .FirstOrDefault(e => e.StandardAttributeId == pa.StandardAttributeId);
                    pa.ProfileVersion = null;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return profileVersion;
        }

        public int CreateProfileVersion(DataSummitModels.DB.ProfileVersions profileVersion)
        {
            int returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.ProfileVersions.Add(profileVersion);
                dataSummitDbContext.SaveChanges();
                returnid = profileVersion.ProfileVersionId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return returnid;
        }
        public void UpdateProfileVersion(int id, DataSummitModels.DB.ProfileVersions profileVersion)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.ProfileVersions.Update(profileVersion);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteProfileVersion(int profileVersionId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.ProfileVersions profileversion = dataSummitDbContext.ProfileVersions.First(p => p.ProfileVersionId == profileVersionId);
                dataSummitDbContext.ProfileVersions.Remove(profileversion);
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
