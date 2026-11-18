using System.Globalization;
using System.Security.Claims;

namespace UTB.Casino.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>Id of the signed-in user, or null when nobody is signed in.</summary>
    public static int? GetUserId(this ClaimsPrincipal principal) =>
        int.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? id : null;
}
