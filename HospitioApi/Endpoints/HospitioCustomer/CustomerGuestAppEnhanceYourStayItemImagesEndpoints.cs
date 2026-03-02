using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.DeleteGuestAppEnhanceYourStayItemsImages;
using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImages;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppEnhanceYourStayItemImagesEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/guest-app-enhance-your-stay-item-images",
        singular: "api/hospitio-customer/guest-app-enhance-your-stay-item-image");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetGuestAppEnhanceYourStayItemImageAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteGuestAppEnhanceYourStayItemImageAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateGuestAppEnhanceYourStayItemImageAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetGuestAppEnhanceYourStayItemImageAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, string? SearchColumn, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int CustomerGuestAppEnhanceYourStayItemId)
    {
        GetGuestAppEnhanceYourStayItemImagesIn @in = new()
        {
            SearchColumn = SearchColumn,
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersGuestAppEnhanceYourStayItemImagesRequest(@in), ct);

    }
    private async Task<IResult> DeleteGuestAppEnhanceYourStayItemImageAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int CustomerGuestAppEnhanceYourStayItemId, CT ct)
    {
        DeleteGuestAppEnhanceYourStayItemsImagesIn @in = new() { CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId };
        return await mtrHlpr.ToResultAsync(new DeleteEnhanceYourStayItemsImageRequest(@in), ct);
    }
    private async Task<IResult> CreateGuestAppEnhanceYourStayItemImageAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGuestAppEnhanceYourStayItemImageIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateGuestAppEnhanceYourStayItemImageRequest(@in), ct);
    #endregion
}