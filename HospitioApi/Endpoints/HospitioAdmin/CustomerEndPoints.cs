using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;
using HospitioApi.Core.HandleCustomers.Commands.DeleteCustomer;
using HospitioApi.Core.HandleCustomers.Commands.ERPCustomerActivation;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerProduct;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerUser;
using HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByIdForHospitio;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomers;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;
using HospitioApi.Core.HandleCustomers.Queries.GetLanguages;
using HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;
using HospitioApi.Helpers;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class CustomerEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-admin/customers",
      singular: "api/hospitio-admin/customer");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomersMainInfoAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/create", CreateCustomerAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/erpCustomerCreate", CreateERPCustomerAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/update", UpdateCustomerAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/updateproduct", UpdateCustomerProductAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/updatecustomeruser", UpdateCustomerUserProductAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}/languages", GetLanguagesAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/languageTransalation", GetLanguageTranslationAsync)
        .RequireAuthorization(),
         app.MapDelete($"/{Route.Singular}" , DeleteCustomerAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Plural}/getcustomerdropdowndetails",GetCustomerDropdownsAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/updateErpServicePack", UpdateERPServicePackAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/updateErpCustomerStatus", UpdateERPCustomerStatusAsync)
        .AllowAnonymous(),

    };

    #region Delegates
    private async Task<IResult> GetCustomersMainInfoAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, string? AlphabetsStartsWith, ClaimsPrincipal cp, CT ct)
    {
        GetCustomersMainInfoIn @in = new()
        {
            SortOrder = SortOrder ?? "ASC",
            SortColumn = SortColumn ?? string.Empty,
            SearchValue = SearchValue ?? string.Empty,
            PageNo = PageNo,
            PageSize = PageSize,
            AlphabetsStartsWith = AlphabetsStartsWith ?? string.Empty,
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersMainInfoRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerByIdForHospitioIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerByIdForHospitioRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerRequest(@in), ct);
    private async Task<IResult> CreateERPCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateERPCustomerIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateERPCustomerRequest(@in), ct);
    private async Task<IResult> UpdateCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerRequest(@in, UserTypeEnum.Hospitio, null), ct);
    private async Task<IResult> UpdateCustomerProductAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerProductIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new UpdateCustomerProductRequest(@in), ct);
    private async Task<IResult> UpdateCustomerUserProductAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerUserIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new UpdateCustomerUserRequest(@in), ct);

    private async Task<IResult> GetLanguagesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetLanguagesRequest(), ct);
    }

    private async Task<IResult> GetLanguageTranslationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] GetLanguageTranslationIn @in, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetLanguageTranslationRequest(true, @in.ChannelId, @in.Message, null), ct);
    }

    private async Task<IResult> DeleteCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerDropdownsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomersRequest(), ct);
    }
    private async Task<IResult> UpdateERPServicePackAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateERPServicePackIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new UpdateERPServicePackRequest(@in), ct);
    private async Task<IResult> UpdateERPCustomerStatusAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] ERPCustomerActivationIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new ERPCustomerActivationRequest(@in), ct);

    #endregion
}
