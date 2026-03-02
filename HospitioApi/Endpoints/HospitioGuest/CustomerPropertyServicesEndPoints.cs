using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceById;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerPropertyServicesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-guest/propertyservices",
        singular: "api/hospitio-guest/propertyservice");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapPost($"/{Route.Singular}",GetCustomerPropertyServiceAsync)
        .AllowAnonymous(),
    };

    private async Task<IResult> GetCustomerPropertyServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] GetCustomerPropertyServiceByIdIn @in, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyServiceByIdRequest(@in), ct);
    }
}
