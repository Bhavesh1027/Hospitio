using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItemById;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItems;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerEnhanceYourStayItemsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/enhance-stay-items",
       singular: "api/hospitio-customer/enhance-stay-item");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}", GetCustomerEnhanceYourStayItemsAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerEnhanceYourStayItemAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerEnhanceYourStayItemAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerEnhanceYourStayItemAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerEnhanceYourStayItemAsync)
        .AllowAnonymous()
        };

    #region Delegates
    private async Task<IResult> GetCustomerEnhanceYourStayItemsAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int CustomerId)
    {
        GetCustomerEnhanceYourStayItemsIn @in = new()
        {
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayItemsRequest(@in), ct);

    }
    private async Task<IResult> GetCustomerEnhanceYourStayItemAsync([FromServices] IMediatorHelper mtrHlpr,ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerEnhanceYourStayItemByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayItemByIdRequest(@in,cp.UserType()), ct);
    }
    private async Task<IResult> DeleteCustomerEnhanceYourStayItemAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerEnhanceYourStayItemIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerEnhanceYourStayItemRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerEnhanceYourStayItemAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerEnhanceYourStayItemIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomerEnhanceYourStayItemRequest(@in), ct);
    }
    private async Task<IResult> UpdateCustomerEnhanceYourStayItemAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomerEnhanceYourStayItemIn @in, ClaimsPrincipal cp,CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new UpdateCustomerEnhanceYourStayItemRequest(@in), ct);
    }

    #endregion
}