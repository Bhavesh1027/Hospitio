using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleConnectionDefinations.Commands.SyncConnectionDefinations;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGr4vy;

public class ConnectionDefinationEndpoint : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-gr4vy/connectiondefinitionssync");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",SyncConnectionDefinitionsAsync)
        .AllowAnonymous(),

    };
    #region Delegates
    private async Task<IResult> SyncConnectionDefinitionsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new SyncConnectionDefinationsRequest(), ct);
    #endregion
}
