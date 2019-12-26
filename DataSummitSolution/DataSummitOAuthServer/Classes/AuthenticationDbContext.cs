using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataSummitOAuthServer
{
    public class AuthenticationDbContext : IdentityDbContext<DataSummitUser>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        { }

        public AuthenticationDbContext() : base()
        { Database.EnsureCreated(); }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
