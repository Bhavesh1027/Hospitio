using System.Security.Claims;

namespace HospitioApi.Shared;

static public class PrincipalExtensions
{
    public static string? UserName(this ClaimsPrincipal user) =>
        user.Identity is not null && user.Identity.IsAuthenticated
            ? user.Identity.Name
            : null;

    public static bool IsAdmin(this ClaimsPrincipal user) =>
        user.IsInRole(Role.SuperAdmin);

    public static string? UserId(this ClaimsPrincipal user) =>
        user.Identity is not null && user.Identity.IsAuthenticated
            ? user.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value
            : null;
    public static string? CustomerId(this ClaimsPrincipal user) =>
       user.Identity is not null && user.Identity.IsAuthenticated
           ? user.Claims.Where(x => x.Type == "CustomerId").FirstOrDefault().Value
           : null;

    public static string? UserType(this ClaimsPrincipal user) =>
        user.Identity is not null && user.Identity.IsAuthenticated
            ? user.Claims.Where(x => x.Type == "UserType").FirstOrDefault().Value
            : null;
}
