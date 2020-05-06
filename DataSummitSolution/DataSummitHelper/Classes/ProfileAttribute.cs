using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class ProfileAttribute
    {
        private DataSummitDbContext dataSummitDbContext;

        public ProfileAttribute(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<ProfileAttributes> GetAllProfileVersionProfileAttributes(int profileVersionId)
        {
            List<ProfileAttributes> profileattributes = new List<ProfileAttributes>();
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

        public ProfileAttributes GetProfileAttributesById(int profileAttributeId)
        {
            ProfileAttributes profileattributes = new ProfileAttributes();
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

        public long CreateProfileAttribute(ProfileAttributes profileAttribute)
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
        public void UpdateProfileAttribute(long id, ProfileAttributes profileAttribute)
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
        public void DeleteProfileAttribute(long profileAttributeId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                ProfileAttributes profileattribute = dataSummitDbContext.ProfileAttributes.First(p => p.ProfileAttributeId == profileAttributeId);
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
