using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataSummitWeb.Classes;
using DataSummitWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace DataSummitWeb
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public static bool IsProdEnvironment = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IServiceCollection Services { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // USE THIS FOR SIMPLE USER NAME AND PASSWORD or SERVER to SERVER comms
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:55836";
                options.RequireHttpsMetadata = false;

                options.Audience = "values";
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies");

            //services.AddCors(options =>
            //{
            //    // this defines a CORS policy called "default"
            //    options.AddPolicy("default", policy =>
            //    {
            //        //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            //        policy.WithOrigins("http://localhost:55837", "http://localhost:56156", "http://localhost:4200");
            //        //policy.WithOrigins("https://oauth.data-summit.co.uk", "https://ui.data-summit.co.uk", "https://data-summit.co.uk")
            //        //    .AllowAnyHeader()
            //        //    .AllowAnyMethod();
            //    });
            //});

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            //This exposes IServiceCollection after ConfigureServices has been called+
            //which allows the database connection to be set depending on the environment settings
            Services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                ConnectionString = Configuration.GetConnectionString("DatabaseConnection");
                Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(ConnectionString));
                IsProdEnvironment = true;
            }
            else
            {
                ConnectionString = Configuration.GetConnectionString("DatabaseConnection");
                Services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(ConnectionString));
            }

            app.UseHttpsRedirection();

            app.UseCors("default");

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}