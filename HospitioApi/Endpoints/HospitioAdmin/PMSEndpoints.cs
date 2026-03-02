using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePMS.Queries.GetPMS;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class PMSEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-admin/pms/");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}"+"{Id}",GetPMSAsync)
        .AllowAnonymous(),

    };
    #region Delegates
    private async Task<IResult> GetPMSAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetPMSRequest(Id), ct);
    #endregion
}
