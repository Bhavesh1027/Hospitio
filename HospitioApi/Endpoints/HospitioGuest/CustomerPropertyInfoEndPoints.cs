using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
using HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerPropertyInfoEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/propertiesinfo",
       singular: "api/hospitio-guest/propertyinfo");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}/Get",GetCustomerPropertyInfoByAppBuilderIdAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}/displayorder-get",GetDisplayOrderAsync)
        .AllowAnonymous(),
     };

    private async Task<IResult> GetCustomerPropertyInfoByAppBuilderIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "AppBuilderIdId")] int AppBuilderIdId,int CustomerId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomersPropertiesInfoByAppBuilderIdIn @in = new() { 
            AppBuilderId = AppBuilderIdId,
            CustomerId= CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersPropertiesInfoByAppBuilderIdRequest(@in, cp.CustomerId()!,UserTypeEnum.Guest), ct);
    }
    private async Task<IResult> GetDisplayOrderAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "ReferenceId")] int ReferenceId, ClaimsPrincipal cp, CT ct)
    {
        GetDisplayOrderIn @in = new() { ReferenceId = ReferenceId };
        return await mtrHlpr.ToResultAsync(new GetDisplayOrderRequest(@in), ct);
    }
}
