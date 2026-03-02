using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;
public class PermissionEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-admin/permission",
        singular: "api/hospitio-admin/permissions");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetPermissionsAsync)
        .AllowAnonymous(),
    };
    #region Delegates
    private async Task<IResult> GetPermissionsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetFeaturePermissionsRequest(), ct);

    #endregion
}