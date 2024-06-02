using System.Security.Claims;

namespace PL.Util
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user) // add newfuncionaltoclass(extension method)
        {
            return Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
