using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.CreateCustomerEnhanceYourStayCategory;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategory;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategories;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoriesWithRelation;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryById;
using HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerEnhanceYourStayCategoriesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/enhance-stay-categories",
       singular: "api/hospitio-customer/enhance-stay-category");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}", GetCustomerEnhanceYourStayCategoriesAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerEnhanceYourStayCategoryAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}/with-relation",GetCustomerEnhanceYourStayCategoriesWithRelationAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerEnhanceYourStayCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerEnhanceYourStayCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerEnhanceYourStayCategoryAsync)
        .AllowAnonymous()
        };

    #region Delegates
    private async Task<IResult> GetCustomerEnhanceYourStayCategoriesAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int CustomerId)
    {
        GetCustomerEnhanceYourStayCategoriesIn @in = new()
        {
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayCategoriesRequest(@in), ct);

    }
    private async Task<IResult> GetCustomerEnhanceYourStayCategoryAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerEnhanceYourStayCategoryByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayCategoryByIdRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerEnhanceYourStayCategoriesWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int CustomerId)
    {
        GetCustomerEnhanceYourStayCategoriesWithRelationIn @in = new()
        {
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayCategoriesWithRelationRequest(@in), ct);

    }
    private async Task<IResult> DeleteCustomerEnhanceYourStayCategoryAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerEnhanceYourStayCategoryIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerEnhanceYourStayCategoryRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerEnhanceYourStayCategoryAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerEnhanceYourStayCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerEnhanceYourStayCategoryRequest(@in), ct);
    private async Task<IResult> UpdateCustomerEnhanceYourStayCategoryAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomerEnhanceYourStayCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerEnhanceYourStayCategoryRequest(@in), ct);
    #endregion
}