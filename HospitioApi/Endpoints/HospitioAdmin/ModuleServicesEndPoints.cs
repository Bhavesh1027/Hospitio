using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;
using HospitioApi.Core.HandleModuleServices.Queries.GetModuleServices;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ModuleServicesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-admin/moduleservices",
        singular: "api/hospitio-admin/moduleservice");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetModuleServicesAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetModuleServiceAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetModuleServicesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetModuleServicesRequest(), ct);
    private async Task<IResult> GetModuleServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetModuleServiceByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetModuleServiceByIdRequest(@in), ct);
    }
    #endregion
}
