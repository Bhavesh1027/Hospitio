using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.EditCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestsCheckInFormEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        singular: "api/hospitio-customer/guest-checkIn");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",GetCustomerGuestsCheckInFormBuilderAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestCheckInFormBuilderAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", EditCustomerGuestCheckInFormBuilderAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestsCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestsCheckInFormBuilderIn @in = new()
        {
            CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestsCheckInFormBuilderRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestsCheckInFormBuilderIn @in, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomerGuestsCheckInFormBuilderRequest(@in), ct);
    }
    private async Task<IResult> EditCustomerGuestCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] EditCustomerGuestsCheckInFormBuilderIn @in, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new EditCustomerGuestsCheckInFormBuilderRequest(@in), ct);
    }

    #endregion
}