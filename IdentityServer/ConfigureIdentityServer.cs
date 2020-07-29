using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public class ConfigureIdentityServer
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),

                new IdentityResource(
                    name: "mv10blog.identity",
                    displayName: "MV10 Blog User Profile",
                    new[] { "mv10_accounttype" })
            };
        }

        static string[] allowedScopes =
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
            "resource1.scope1",
            "resource2.scope1",
            "transaction"
        };

        public static IEnumerable<Client> GetClients()
        {
            var secret = "the_secret".Sha256();
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mv10blog.client",
                    ClientName = "McGuireV10.com",
                    ClientUri = "http://localhost:5002",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret(secret)},
                    AllowRememberConsent = true,
                    AllowOfflineAccess = true,
                    RequirePkce = true,
                    RedirectUris = { "http://localhost:5002/signin-oidc"}, // after login
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc"}, // after logout
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Address,
                        "mv10blog.identity"
                    }
                },
                new Client
                {
                    ClientId = "mvc.code",
                    ClientName = "MVC Code Flow",
                    ClientUri = "http://identityserver.io",

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RequireConsent = true,
                    RequirePkce = true,
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44302/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44302/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44302/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = allowedScopes
                },

            };
        }
    }
}
