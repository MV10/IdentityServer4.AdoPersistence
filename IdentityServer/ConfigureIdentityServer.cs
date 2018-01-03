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
                    claimTypes: new[] { "mv10_accounttype" })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mv10blog.client",
                    ClientName = "McGuireV10.com",
                    ClientUri = "http://localhost:5002",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = {new Secret("the_secret".Sha256())},
                    AllowRememberConsent = true,
                    AllowOfflineAccess = true,
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
                }
            };
        }
    }
}
