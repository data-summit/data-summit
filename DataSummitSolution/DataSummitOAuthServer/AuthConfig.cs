using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitOAuthServer
{
    public class AuthConfig
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("readonly", "DataSummit API"),
                new ApiResource("values", "DataSummit API"),
                new ApiResource("sysadmin", "DataSummit API")
            };
        }

        // test example clients. In reality these will come from the DB
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Can request either Server-Server or Username/Password auth token
                new Client
                {
                    Enabled = true,
                    ClientId = "postmanUserPwd",
                    ClientName = "postmanUserPwd",

                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours 

                    // no interactive user, use the user and password for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("DataSummitUISecret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "values", "sysadmin" }
                },

                // OpenID Connect implicit flow client (MVC) or Client Credentials (Server-Server)
                new Client
                {
                    Enabled = true,
                    ClientId = "DataSummitUI",
                    ClientName = "DataSummitUI",
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:56155/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:56155/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                    }
                },
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            //var users = GetUsers();

            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                //new TestUser
                //{
                //    SubjectId = "2",
                //    Username = users.FirstOrDefault().UserName,
                //    Password = "password"
                //}
            };
        }

        public static List<DataSummitUser> GetUsers()
        {
            var dbContext = new AuthenticationDbContext();

            var users = dbContext.Users.ToList();

            return users;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }
    }
}