using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.CreateCustomerGuestAlerts;
using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlerts;
using HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;
using HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertById;
using HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlerts;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAlertsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/guestalerts",
        singular: "api/hospitio-customer/guestalert");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetGuestAlertsAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetGuestAlertAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteGuestAlertAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateGuestAlertAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateGuestAlertAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetGuestAlertsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAlertsRequest(cp.CustomerId()!), ct);
    }
    private async Task<IResult> GetGuestAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerGuestAlertByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAlertByIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteGuestAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerGuestAlertsIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerGuestAlertsRequest(@in), ct);
    }
    private async Task<IResult> CreateGuestAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerGuestAlertsRequest(@in, cp.CustomerId()!), ct);
    private async Task<IResult> UpdateGuestAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerGuestAlertsRequest(@in, cp.CustomerId()!), ct);
    #endregion
}
