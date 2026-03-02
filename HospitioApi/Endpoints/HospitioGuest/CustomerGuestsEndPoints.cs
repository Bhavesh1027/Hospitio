using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestToken;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerLatitudeLongitude;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-guest/guests");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",GetCustomerGuestToken)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/getCustomerLatitudeLongitude",GetCustomerLatitudeLongitudeAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestToken([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string id)
    {
        GetCustomerGuestTokenIn @in = new()
        {
            Id = id
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestTokenRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerLatitudeLongitudeAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int customerId , int builderId)
    {
        GetCustomerLatitudeLongitudeIn @in = new()
        {
            CustomerId = customerId,
            BuilderId = builderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerLatitudeLongitudeRequest(@in), ct);
    }
    #endregion
}
