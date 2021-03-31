using System;
using System.IdentityModel.Tokens.Jwt;
using DataSummitHelper.Dao;
using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitHelper.Services;
using DataSummitHelper.Services.MachineLearning;
using DataSummitModels.DB;
using DataSummitWeb.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DataSummitWeb
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
            services.AddMvcCore()
                .AddAuthorization()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            var connectionString = Configuration["DatabaseConnection"];
            connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DataSummitDB;Persist Security Info=False;Integrated Security=SSPI;"; 
            // User ID=lightosDB;Password=!Aa12345";

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

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                // CORS has been added to IIS
                    // <repo_path>\DataSummit\data-summit\DataSummitSolution\.vs\DataSummit\config\applicationhost.config
                    //
                    //< customHeaders >
                    //  < clear />
                    //  < add name = "X-Powered-By" value = "ASP.NET" />
                    //         < add name = "Access-Control-Allow-Origin" value = "*" />
                    //            < add name = "Access-Control-Allow-Headers" value = "Content-Type" />
                    //               < add name = "Access-Control-Allow-Methods" value = "GET, POST, PUT, DELETE, OPTIONS" />
                    //                </ customHeaders >
                options.AddPolicy("default", policy =>
                    {
                        //        //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                        //        policy.WithOrigins("http://localhost:55837", "http://localhost:56156", "http://localhost:4200",
                        //"https://oauth.data-summit.co.uk", "https://ui.data-summit.co.uk", "https://data-summit.co.uk")
                        //        //    .AllowAnyHeader()
                        //        //    .AllowAnyMethod();
                    });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add service dependencies.
            services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString))
                .AddDbContext<DataSummitDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString),
                ServiceLifetime.Transient)
                .AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

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

            //Data Summit specific dependency injection services
            services.AddTransient<IAzureResourcesService, AzureResourcesService>();
            services.AddTransient<IDataSummitCompaniesService, DataSummitCompaniesService>();
            services.AddTransient<IDataSummitDocumentsService, DataSummitDocumentsService>();
            services.AddTransient<IDataSummitHelperService, DataSummitHelperService>();
            services.AddTransient<IDataSummitProjectsService, DataSummitProjectsService>();
            services.AddTransient<IDataSummitPropertiesService, DataSummitPropertiesService>();
            services.AddTransient<IDataSummitTemplateAttributesService, DataSummitTemplateAttributesService>();
            services.AddTransient<IDataSummitTemplatesService, DataSummitTemplatesService>();
            services.AddTransient<IClassificationService, ClassificationService>();
            services.AddTransient<IObjectDetectionService, ObjectDetectionService>();
            services.AddTransient<IDataSummitAzureUrlsDao, DataSummitDao>();
            services.AddTransient<IDataSummitCompaniesDao, DataSummitDao>();
            services.AddTransient<IDataSummitDocumentsDao, DataSummitDao>();
            services.AddTransient<IDataSummitMachineLearningDao, DataSummitDao>();
            services.AddTransient<IDataSummitProjectsDao, DataSummitDao>();
            services.AddTransient<IDataSummitPropertiesDao, DataSummitDao>();
            services.AddTransient<IDataSummitTemplateAttributesDao, DataSummitDao>();
            services.AddTransient<IDataSummitTemplatesDao, DataSummitDao>();

            // For .Net Core 2.2 to 3.1 update this was added as per the issues detailed here:
            // https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing
            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)   //IHostingEnvironment to be deprecated
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("default");

            app.UseAuthentication();

            // For .Net Core 2.2 to 3.1 update this was added as per the issues detailed here:
            // https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing
            //app.UseMvc(); //Removed
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}