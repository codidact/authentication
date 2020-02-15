using System.Collections.Generic;
using IdentityServer4.Models;

namespace Codidact.Authentication.Infrastructure
{
    public class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "codidact.com",
                    ClientSecrets = { new Secret("foo".Sha256()), },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "openid" },
                },
            };
        }
    }
}
