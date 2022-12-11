using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace AuthenticationPratice.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetNameIdentity(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
