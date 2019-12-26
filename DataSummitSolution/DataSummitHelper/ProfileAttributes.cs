using DataSummitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class ProfileAttributes
    {
        private DataSummitDbContext dataSummitDbContext;

        public ProfileAttributes(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.ProfileAttributes> GetAllProfileVersionProfileAttributes(int profileVersionId, bool IsProdEnvironment = false)
        {
            List<DataSummitModels.ProfileAttributes> profileattributes = new List<DataSummitModels.ProfileAttributes>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                profileattributes = dataSummitDbContext.ProfileAttributes.Where(e => e.ProfileVersionId == profileVersionId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return profileattributes;
        }

        public DataSummitModels.ProfileAttributes GetProfileAttributesById(int profileAttributeId)
        {
            DataSummitModels.ProfileAttributes profileattributes = new DataSummitModels.ProfileAttributes();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                profileattributes = dataSummitDbContext.ProfileAttributes.First(e => e.ProfileAttributeId == profileAttributeId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return profileattributes;
        }

        public long CreateProfileAttribute(DataSummitModels.ProfileAttributes profileAttribute, bool IsProdEnvironment = false)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                profileAttribute.CreatedDate = DateTime.Now;
                //profileattribute.UserId = OAuthServer.User.Identity.Name;
                dataSummitDbContext.ProfileAttributes.Add(profileAttribute);
                dataSummitDbContext.SaveChanges();
                returnid = profileAttribute.ProfileAttributeId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }
        public void UpdateProfileAttribute(long id, DataSummitModels.ProfileAttributes profileAttribute, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.ProfileAttributes.Update(profileAttribute);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteProfileAttribute(long profileAttributeId, bool IsProdEnvironment = false)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.ProfileAttributes profileattribute = dataSummitDbContext.ProfileAttributes.First(p => p.ProfileAttributeId == profileAttributeId);
                dataSummitDbContext.ProfileAttributes.Remove(profileattribute);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
