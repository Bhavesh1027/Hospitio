using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleModules.Queries.GetModules;
using HospitioApi.Core.HandleModules.Queries.GetModulesDapperDemo;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ModuleEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/modules");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetModulesAsync)
        .AllowAnonymous(),

          app.MapGet($"/{Route.Plural}/dapper",GetModulesByDapperAsync)
        .AllowAnonymous(),

    };
    #region Delegates
    private async Task<IResult> GetModulesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetModulesRequest(), ct);


    private async Task<IResult> GetModulesByDapperAsync(
        [FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int pageNo, int pageSize)
    {
        GetModulesDapperDemoIn @in = new()
        {
            PageNo = pageNo,
            PageSize = pageSize
        };
        return await mtrHlpr.ToResultAsync(new GetModulesDapperDemoRequest(@in), ct);

    }

    #endregion

}
