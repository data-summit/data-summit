using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthServer
{
    public class AuthConfig
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("readonly", "DataSummit API"),
                new ApiResource("sysadmin", "DataSummit API")
            };
        }

        // test example clients. In reality these will come from the DB
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client // Client credentials auth
                {
                    Enabled = true,
                    ClientId = "DataSummitWebUI",
                    ClientName = "DataSummitWebUI",
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours 
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // this means use clientId/clientSecret for authentication
                    ClientSecrets = { new Secret("DataSummitUISecret".Sha256()) }, // Send the UNHASHED value in POSTMAN
                    AllowedScopes = { "readonly" },
                    AllowedCorsOrigins = { "http://localhost:4200" }
                },
                new Client // Client credentials auth
                {
                    Enabled = true,
                    ClientId = "DataSummitUI",
                    ClientName = "DataSummitUI",
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours 
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // this means use clientId/clientSecret for authentication
                    ClientSecrets = { new Secret("DataSummitUISecret".Sha256()) }, // Send the UNHASHED value in POSTMAN
                    AllowedScopes = { "readonly" },
                },
                new Client // Username and password auth
                {
                    Enabled = true,
                    ClientId = "userpwd",
                    ClientName = "userpwd",
                    AccessTokenLifetime = 60 * 60 * 2, //2 hours 
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // this means username and password e.g. from login form
                    ClientSecrets = { new Secret("DataSummitUISecret".Sha256()) }, // Send the UNHASHED value in POSTMAN
                    AllowedScopes = { "sysadmin" }
                },
                new Client // OpenID Connect auth
                {
                    Enabled = true,
                    ClientId = "OIDC",
                    ClientName = "OIDC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris           = { "http://localhost:55836/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:55836/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "sysadmin"
                    },
                },
                new Client // OpenID Connect Hybrid auth 
                {
                    ClientId = "OIDCHybrid",
                    ClientName = "OIDCHybrid Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("DataSummitUISecret".Sha256())
                    },

                    RedirectUris           = { "http://localhost:55836/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:55836/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sysadmin"
                    },
                    AllowOfflineAccess = true
                },
                new Client // ASP Net Identity for users
                {
                    ClientId = "AspNetIdentity",
                    ClientName = "AspNetIdentity Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("DataSummitUISecret".Sha256())
                    },

                    RedirectUris           = { "http://localhost:55836/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:55836/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sysadmin"
                    },
                    AllowOfflineAccess = true
                },
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }

        public static List<DataSummitUser> GetUsers()
        {
            return new List<DataSummitUser>
            {
                new DataSummitUser
                {
                    Id = "1",
                    UserName = "test1",
                    Password = "password1",
                    PasswordHash = "password1".Sha256(),
                },
                new DataSummitUser
                {
                    Id = "2",
                    UserName = "test2",
                    Password = "password2",
                    PasswordHash = "password2".Sha256(),
                }
            };
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