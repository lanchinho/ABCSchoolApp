using ABCShared.Library.Constants;
using System.Security.Claims;

namespace App.Infrastructure.Extensions;
public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.Email).Value ?? string.Empty;

    public static string GetFirstName(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.Name).Value ?? string.Empty;

    public static string GetLastName(this ClaimsPrincipal principal) 
        => principal.FindFirst(ClaimTypes.Surname).Value ?? string.Empty;

    public static string GetTenant(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimConstants.Tenant).Value ?? string.Empty;
    public static string GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.NameIdentifier).Value ?? string.Empty;

    public static string GetPhoneNumber(this ClaimsPrincipal principal)
    => principal.FindFirst(ClaimTypes.MobilePhone).Value ?? string.Empty;
}
