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
    }
}
