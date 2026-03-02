using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
using HospitioApi.Helpers;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class UserLevelsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-admin/userlevels");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetUserLevelsAsync)
        .AllowAnonymous()
    };

    #region Delegate
    private async Task<IResult> GetUserLevelsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetUserLevelsRequest(UserTypeEnum.Hospitio), ct);
    #endregion
}
