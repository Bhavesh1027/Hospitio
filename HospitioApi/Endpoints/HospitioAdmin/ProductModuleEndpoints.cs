using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleProductModule.Commands.CreateProductModule;
using HospitioApi.Core.HandleProductModule.Commands.EditProductModule;
using HospitioApi.Core.HandleProductModule.Queries.GetProductModuleById;
using HospitioApi.Core.HandleProductModule.Queries.GetProductModules;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ProductModuleEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(plural__: "api/hospitio-admin/productmodules/");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        //app.MapPost($"/{Route.Plural}create", CreateAsync)
        //    .RequireAuthorization(),

        //app.MapPost($"/{Route.Plural}" + "{Id}/edit", EditAsync)
        //    .RequireAuthorization(),

        app.MapGet($"/{Route.Plural}" +"{Id}", GetByIdAsync)
            .RequireAuthorization(),

          app.MapGet($"/{Route.Plural}" , GetAllAsync)
            .RequireAuthorization(),
    };

    #region Delegates
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateProductModuleIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateProductModuleRequest(@in), ct);

    private async Task<IResult> EditAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, [FromBody] EditProductModuleIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditProductModuleRequest(@in, Id), ct);

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductModuleByIdRequest(Id), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductModulesRequest(), ct);

    #endregion
}