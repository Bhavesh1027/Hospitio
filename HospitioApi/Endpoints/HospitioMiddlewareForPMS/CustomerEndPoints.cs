using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomersByGuIds;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioMiddlewareForPMS;

public class CustomerEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-middleware/customers",
       singular: "api/hospitio-middleware/customer");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",GetCustomerAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}",GetAsync)
        .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "CustomerGuId")] Guid CustomerGuId, CT ct)
    {
        GetCustomerByGuIdIn @in = new() { GuId = CustomerGuId };
        return await mtrHlpr.ToResultAsync(new GetCustomerByGuIdForHospitioRequest(@in), ct);
    }
    private async Task<IResult> GetAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] string guids, ClaimsPrincipal cp, CT ct)
    {
        var guidList = guids.Split(',').Select(Guid.Parse).ToList();
        GetCustomersByGuIdsIn @in = new() { guids = guidList };
        return await mtrHlpr.ToResultAsync(new GetCustomersByGuIdsRequest(@in), ct);
    }
    #endregion
}
