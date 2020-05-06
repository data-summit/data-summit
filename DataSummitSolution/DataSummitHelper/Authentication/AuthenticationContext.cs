using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataSummitHelper
{
    public class AuthenticationContext : IdentityDbContext<DataSummitUser>
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        { }

        public AuthenticationContext() : base()
        { Database.EnsureCreated(); }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = "Server=tcp:datasummit.database.windows.net,1433;Initial Catalog=DataSummitDB;Persist Security Info=False;User ID=TomJames;Password=!Aa1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //"Data Source=(localdb)\\ProjectsV13;Initial Catalog=DataSummitDB;Integrated Security=True;Connect Timeout=30;";

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(conn);
            }
        }
    }
}
