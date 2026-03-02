using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerById;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerRoomNamesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/roomnames");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapGet($"/{Route.Plural}",GetAsync)
        .RequireAuthorization(),
    };


    #region Delegates
    private async Task<IResult> GetAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomerRoomNamesRequest(cp.CustomerId()!), ct);
    }
    #endregion
}
