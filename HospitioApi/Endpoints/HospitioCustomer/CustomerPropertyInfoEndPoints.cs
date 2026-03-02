using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfo;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfo;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfo;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoById;
using HospitioApi.Core.HandleDisplayorder.Commands.UpdateDisplayorder;
using HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPropertyInfoEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/propertiesinfo",
       singular: "api/hospitio-customer/propertyinfo");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerPropertiesInfoAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerPropertyInfoAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/Get",GetCustomerPropertyInfoByAppBuilderIdAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerPropertyInfoAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}/displayorder-get",GetDisplayOrderAsync)
        .AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerPropertyInfoAsync)
        //.AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/displayorder-update", UpdateStatusInfoAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/createupdate", UpdateCustomerPropertyInfoAsync)
        .RequireAuthorization()
     };

#region Delegates
private async Task<IResult> GetCustomerPropertiesInfoAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, string? SearchColumn, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int CustomerId)
    {
        GetCustomersPropertiesInfoIn @in = new()
        {
            SearchColumn = SearchColumn,
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersPropertiesInfoRequest(@in), ct);

    }
    private async Task<IResult> GetCustomerPropertyInfoAsync([FromServices] IMediatorHelper mtrHlpr,ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomersPropertiesInfoByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomersPropertiesInfoByIdRequest(@in,cp.UserType()), ct);
    }
    private async Task<IResult> GetCustomerPropertyInfoByAppBuilderIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "AppBuilderIdId")] int AppBuilderIdId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomersPropertiesInfoByAppBuilderIdIn @in = new() { AppBuilderId = AppBuilderIdId };
        return await mtrHlpr.ToResultAsync(new GetCustomersPropertiesInfoByAppBuilderIdRequest(@in,cp.CustomerId()!,UserTypeEnum.Customer), ct);
    }
    private async Task<IResult> GetDisplayOrderAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "ReferenceId")] int ReferenceId, ClaimsPrincipal cp, CT ct)
    {
        GetDisplayOrderIn @in = new() { ReferenceId = ReferenceId };
        return await mtrHlpr.ToResultAsync(new GetDisplayOrderRequest(@in), ct);
    }
    
    private async Task<IResult> DeleteCustomerPropertyInfoAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomersPropertiesInfoIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomersPropertiesInfoRequest(@in), ct);
    }
    //private async Task<IResult> CreateCustomerPropertyInfoAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomersPropertiesInfoIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreateCustomersPropertiesInfoRequest(@in), ct);
    private async Task<IResult> UpdateCustomerPropertyInfoAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomersPropertiesInfoIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomersPropertiesInfoRequest(@in,cp.CustomerId()!), ct);
    private async Task<IResult> UpdateStatusInfoAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromForm] UpdateDisplayorderIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateDisplayorderRequest(@in), ct);
    #endregion
}