using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class BusinessTypesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-admin/businesstypes");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetBusinessTypesAsync)
        .AllowAnonymous(),

    };
    #region Delegates
    private async Task<IResult> GetBusinessTypesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetBusinessTypesRequest(), ct);
    #endregion
}
