using System.Security.Claims;

namespace Social.Domain.Extensions;

public static class ClaimsPrincipalExtension
{
    public static long GetCurrentUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.Claims.First(c => c.Type == "id");
        return long.Parse(claim.Value);
    }
}