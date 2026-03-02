using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestAppRoomServiceEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/roomservices",
       singular: "api/hospitio-guest/roomservice");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}/get-with-relation",GetCustomerRoomServiceWithRelationAsync)
        .AllowAnonymous()
    };

    private async Task<IResult> GetCustomerRoomServiceWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerRoomServiceWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerRoomServiceWithRelationRequest(@in, cp.CustomerId()!, cp.UserType()!), ct);

    }
}
