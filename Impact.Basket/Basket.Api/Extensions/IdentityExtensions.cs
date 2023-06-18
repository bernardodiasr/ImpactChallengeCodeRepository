using System.Security.Claims;
using System.Security.Principal;

namespace Basket.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this IIdentity identity)
        {
            return (identity as ClaimsIdentity)?.Claims.SingleOrDefault((Claim c) => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
