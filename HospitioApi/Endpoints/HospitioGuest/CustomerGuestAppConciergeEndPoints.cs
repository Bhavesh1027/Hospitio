using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestAppConciergeEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/concierges",
       singular: "api/hospitio-guest/concierge");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerConciergeWithRelationAsync)
        .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetCustomerConciergeWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerConciergeWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerConciergeWithRelationRequest(@in, cp.CustomerId()!,cp.UserType()!), ct);
    }
    #endregion
}

