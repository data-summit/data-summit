using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class AuthenticationRepository : IDisposable
    {
        private AuthenticationContext context;

        private UserManager<DataSummitUser> userManager;

        public AuthenticationRepository()
        {
            context = new AuthenticationContext();
            userManager = new UserManager<DataSummitUser>(new UserStore<DataSummitUser>(context),
                null, new PasswordHasher<DataSummitUser>(), null, null, null, null, null, null);
        }

        public async Task<IdentityResult> RegisterUser(DataSummitUser dataSummitUser, string password)
        {
            var result = IdentityResult.Failed();

            try
            {
                result = await userManager.CreateAsync(dataSummitUser, password);
            }
            catch (Exception ex)
            {
                result = IdentityResult.Failed(new IdentityError { Code = "1", Description = ex.Message });
            }

            return result;
        }

        public async Task<DataSummitUser> FindUser(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);

            return user;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();
        }
    }
}
