using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleProduct.Commands.CreateProduct;
using HospitioApi.Core.HandleProduct.Commands.EditProduct;
using HospitioApi.Core.HandleProduct.Queries.GetProductById;
using HospitioApi.Core.HandleProduct.Queries.GetProducts;
using HospitioApi.Core.HandleProductNames.Queries.GetProductNames;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ProductEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(plural__: "api/hospitio-admin/products/");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Plural}Save", CreateAsync)
            .RequireAuthorization(),

        //app.MapPost($"/{Route.Plural}" + "{Id}/edit", EditAsync)
        //    .RequireAuthorization(),

        app.MapGet($"/{Route.Plural}" +"{Id}", GetByIdAsync)
            .RequireAuthorization(),

          app.MapGet($"/{Route.Plural}" , GetAllAsync)
            .RequireAuthorization(),

          app.MapGet($"/{Route.Plural}Name",GetProductNamesAsync)
        .RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateProductIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateProductRequest(@in), ct);

    private async Task<IResult> EditAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, [FromBody] EditProductIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditProductRequest(@in, Id), ct);

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductByIdRequest(Id, cp.UserId()!), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, CT ct)
    {
        GetProductsIn @in = new()
        {
            SearchValue = SearchValue ?? "",
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn ?? "",
            SortOrder = SortOrder ?? "",
        };

        return await mtrHlpr.ToResultAsync(new GetProductsRequest(@in), ct);
    }

    private async Task<IResult> GetProductNamesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetProductNamesRequest(), ct);
    #endregion
}