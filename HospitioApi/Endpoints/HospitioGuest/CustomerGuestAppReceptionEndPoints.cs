using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestAppReceptionEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-guest/receptions",
      singular: "api/hospitio-guest/reception");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerReceptionWithRelationAsync)
        .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetCustomerReceptionWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, ClaimsPrincipal cp, int AppBuilderId)
    {
        GetCustomerReceptionWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerReceptionWithRelationRequest(@in, cp.CustomerId()!, cp.UserType()!), ct);
    }

    #endregion
}
