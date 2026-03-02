using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleFeaturePermission.Queries.GetCustomerFeaturePermissions;
using HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
using HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
using HospitioApi.Helpers;
using System.Drawing.Printing;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPermissionEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-customer/permission",
      singular: "api/hospitio-customer/permissions"
    );

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetCustomerPermissionsAsync)
        .AllowAnonymous(),
    };

    #region Delegates
    private async Task<IResult> GetCustomerPermissionsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetCustomerFeaturePermissionsRequest(), ct);

    #endregion
}
