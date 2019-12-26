using DataSummitOAuthServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace OAuthServer
{
    public class AuthenticationRepository : IDisposable
    {
        private AuthenticationDbContext context;

        private UserManager<DataSummitUser> userManager;

        public AuthenticationRepository()
        {
            context = new AuthenticationDbContext();
            userManager = new UserManager<DataSummitUser>(new UserStore<DataSummitUser>(context),
                null, new PasswordHasher<DataSummitUser>(), null, null, null, null, null, null);
        }

        public async Task<IdentityResult> RegisterUser(string userName, string password)
        {
            var user = new DataSummitUser
            {
                UserName = userName
            };

            var result = await userManager.CreateAsync(user, password);

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
