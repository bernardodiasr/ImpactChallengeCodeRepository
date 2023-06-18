using Basket.Api.Extensions;
using Impact.Core.Contracts;
using System.Security.Claims;

namespace Basket.Api.Security
{
    public class AspNetCoreClaimsIdentityProvider : IIdentityProvider
    {
        private readonly ClaimsPrincipal claimsPrincipal;

        public string Email => claimsPrincipal?.Identity.GetEmail();

        public bool IsAuthenticated
        {
            get
            {
                if (claimsPrincipal == null)
                {
                    return false;
                }

                return true;
            }
        }

        public AspNetCoreClaimsIdentityProvider(IHttpContextAccessor httpContextAccessor)
        {
            claimsPrincipal = httpContextAccessor.HttpContext?.User;
        }
    }
}
