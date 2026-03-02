using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerById;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;
using HospitioApi.Core.HandleCustomers.Queries.GetGuestDefaultCheckinDetails;
using HospitioApi.Core.HandleCustomers.Queries.GetLanguages;
using HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/customers",
       singular: "api/hospitio-customer/customer");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",GetCustomerAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/languages", GetLanguagesAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/languageTransalation", GetLanguageTranslationAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/Currency",GetCustomerCurrencyAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/getcustomername" , GetCustomerNameAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/getguestcheckindetails" , GetGuestCheckInDetailsAsync)
        .RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> GetCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomerByIdRequest(null, cp.CustomerId()!), ct);
    }
    private async Task<IResult> CreateCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerRequest(@in), ct);
    private async Task<IResult> UpdateCustomerAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerRequest(@in, UserTypeEnum.Customer, cp.CustomerId()!), ct);

    private async Task<IResult> GetLanguagesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetLanguagesRequest(), ct);
    }

    private async Task<IResult> GetLanguageTranslationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] GetLanguageTranslationIn @in, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetLanguageTranslationRequest(false, @in.ChannelId, @in.Message, cp.CustomerId()!), ct);
    }
    private async Task<IResult> GetCustomerCurrencyAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerCurrencyIn @in = new()
        {
            Id = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerCurrencyByIdRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerNameAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerNameIn @in = new()
        {
            Id = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerNameRequest(@in) , ct);
    }
    private async Task<IResult> GetGuestCheckInDetailsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetGuestDefaultCheckinDetailsIn @in = new()
        {
            CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
        };
        return await mtrHlpr.ToResultAsync(new GetGuestDefaultCheckinDetailsRequest(@in), ct);
    }

    #endregion
}
