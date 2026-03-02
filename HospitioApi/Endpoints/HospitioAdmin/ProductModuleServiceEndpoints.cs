using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleProductModuleService.Commands.CreateProductModuleService;
using HospitioApi.Core.HandleProductModuleService.Commands.EditProductModuleService;
using HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServiceById;
using HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServices;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ProductModuleServiceEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(plural__: "api/hospitio-admin/productmoduleservices/");

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
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateProductModuleServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateProductModuleServiceRequest(@in), ct);

    private async Task<IResult> EditAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, [FromBody] EditProductModuleServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditProductModuleServiceRequest(@in, Id), ct);

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductModuleServiceByIdRequest(Id), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductModuleServicesRequest(), ct);

    #endregion
}