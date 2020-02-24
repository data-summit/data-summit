using DataSummitModels;
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

        public List<DataSummitModels.ProfileVersions> GetAllCompanyProfileVersions(int companyId)
        {
            List<DataSummitModels.ProfileVersions> profileversions = new List<DataSummitModels.ProfileVersions>();
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

        public DataSummitModels.ProfileVersions GetProfileVersion(int profileVersionId)
        {
            DataSummitModels.ProfileVersions profileVersion = new DataSummitModels.ProfileVersions();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();

                profileVersion = dataSummitDbContext.ProfileVersions
                                    .FirstOrDefault(e => e.ProfileVersionId == profileVersionId);
                profileVersion.ProfileAttributes = dataSummitDbContext.ProfileAttributes
                                    .Where(e => e.ProfileVersionId == profileVersion.ProfileVersionId).ToList();

                foreach (DataSummitModels.ProfileAttributes pa in profileVersion.ProfileAttributes)
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

        public int CreateProfileVersion(DataSummitModels.ProfileVersions profileVersion)
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
        public void UpdateProfileVersion(int id, DataSummitModels.ProfileVersions profileVersion)
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
                DataSummitModels.ProfileVersions profileversion = dataSummitDbContext.ProfileVersions.First(p => p.ProfileVersionId == profileVersionId);
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
