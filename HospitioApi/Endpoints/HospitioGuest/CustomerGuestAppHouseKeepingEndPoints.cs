using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestAppHouseKeepingEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/house-keepings",
       singular: "api/hospitio-guest/house-keeping");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerHouseKeepingWithRelationAsync)
        .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetCustomerHouseKeepingWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerHouseKeepingWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerHouseKeepingWithRelationRequest(@in, cp.CustomerId()!, cp.UserType()!), ct);
    }
    #endregion
}
