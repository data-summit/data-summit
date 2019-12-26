using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthServer
{
    public class AuthenticationRepository : IDisposable
    {
        private AuthenticationDbContext context;

        private UserManager<IdentityUser> userManager;

        public AuthenticationRepository()
        {
            context = new AuthenticationDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context),
                null, new PasswordHasher<IdentityUser>(), null, null, null, null, null, null);
        }

        public async Task<IdentityResult> RegisterUser(DataSummitUser account)
        {
            var user = new IdentityUser
            {
                UserName = account.UserName
            };

            var result = await userManager.CreateAsync(user, account.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
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
