using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlerts;
using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlerts;
using HospitioApi.Core.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlerts;
using HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsByCustomerId;
using HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsById;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerStaffAlertsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-customer/staff-alerts",
    singular: "api/hospitio-customer/staff-alert");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetStaffAlertsAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetStaffAlertAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteStaffAlertAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateStaffAlertAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateStaffAlertAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetStaffAlertsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetCustomerStaffAlertsByCustomerIdRequest(cp.CustomerId()!), ct);
    }
    private async Task<IResult> GetStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerStaffAlertsByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerStaffAlertsByIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerStaffAlertsIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerStaffAlertsRequest(@in), ct);
    }
    private async Task<IResult> CreateStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerStaffAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerStaffAlertsRequest(@in, cp.CustomerId()!), ct);
    private async Task<IResult> UpdateStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerStaffAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerStaffAlertsRequest(@in, cp.CustomerId()!), ct);
    #endregion
}
