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

        //public List<DataSummitModels.ProfileVersions> GetAllProjectProfileVersions(int companyId, int projectId)
        //{
        //    List<DataSummitModels.ProfileVersions> profileversions = new List<DataSummitModels.ProfileVersions>();
        //    try
        //    {
        //        if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
        //        foreach(DataSummitModels.Projects p in dataSummitDbContext.Projects)
        //        {
        //            if (p.CompanyId == companyId && p.ProjectId == projectId)
        //            {
        //                profileversions.Add(p) 
        //                    }
        //        }
        //        profileversions = dataSummitDbContext.ProfileVersions
        //                            .Where(e => e.Company.Projects.Where(f => f.ProjectId == projectId && f.CompanyId == companyId))
        //    }
        //    catch (Exception ae)
        //    {
        //        string strError = ae.Message.ToString();
        //    }
        //    return profileversions;
        //}

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
