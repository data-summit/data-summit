using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace OAuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<DataSummitUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(AuthConfig.GetIdentityResources())
                .AddInMemoryApiResources(AuthConfig.GetApiResources())
                .AddInMemoryClients(AuthConfig.GetClients())
                //.AddAspNetIdentity<DataSummitUser>(); // Get users from database
                .AddTestUsers(AuthConfig.GetTestUsers()); /**** TESTING Auth with test user names and passwords without DB****/

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", options => // OIDC Implicit
            //{
            //    options.SignInScheme = "Cookies";

            //    options.Authority = "http://localhost:55836";
            //    options.RequireHttpsMetadata = false;

            //    options.ClientId = "OIDC";
            //    options.SaveTokens = true;
            //});
            .AddOpenIdConnect("oidc", options => // OIDC Hybrid
             {
                 options.SignInScheme = "Cookies";

                 options.Authority = "http://localhost:55836";
                 options.RequireHttpsMetadata = false;

                 options.ClientId = "OIDCHybrid";
                 options.ClientSecret = "DataSummitUISecret";
                 options.ResponseType = "code id_token";

                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.Scope.Add("sysadmin");
                 options.Scope.Add("offline_access");
             });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddTraceSource("OAuth");

            app.UseIdentityServer();
            app.UseMvc();

        }
    }
}
