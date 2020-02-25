using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Users
    {
        private DataSummitDbContext dataSummitDbContext;
        private readonly AuthenticationRepository authenticationRepository;

        public Users(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext ?? new DataSummitDbContext();

            authenticationRepository = new AuthenticationRepository();
        }

        public List<AspNetUsers> GetAllCompanyUsers(int companyId)
        {
            List<AspNetUsers> Users = new List<AspNetUsers>();
            try
            {
                Users = dataSummitDbContext.AspNetUsers.Where(e => e.CompanyId == companyId).ToList();
            }
            catch (Exception ae)
            {
                string strMessage = ae.Message.ToString();
                string strInner = ae.InnerException.ToString();
            }
            return Users;
        }

        public async Task<long> CreateUsers(AspNetUsers users, DataSummitUser dataSummitUsers, string password)
        {
            long returnid = 0;

            try
            {
                await authenticationRepository.RegisterUser(dataSummitUsers, password);
            }
            catch (Exception ae)
            {
                returnid = - 1;
                string strMessage = ae.Message.ToString();
                string strInner = ae.InnerException.ToString();
            }

            return returnid;
        }

        public void UpdateUsers(string id, AspNetUsers users)
        {
            try
            {
                dataSummitDbContext.AspNetUsers.Update(users);
            }
            catch (Exception ae)
            {
                string strMessage = ae.Message.ToString();
                string strInner = ae.InnerException.ToString();
            }
            return;
        }

        public void DeleteUsers(string aspNetUsersId)
        {
            try
            {
                AspNetUsers aspNetUsers = dataSummitDbContext.AspNetUsers.First(p => p.Id == aspNetUsersId);
                dataSummitDbContext.AspNetUsers.Remove(aspNetUsers);
            }
            catch (Exception ae)
            {
                string strMessage = ae.Message.ToString();
                string strInner = ae.InnerException.ToString();
            }
            return;
        }
    }
}
