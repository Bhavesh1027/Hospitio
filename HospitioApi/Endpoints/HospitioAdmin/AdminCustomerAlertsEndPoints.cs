using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;
using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;
using HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;
using HospitioApi.Core.HandleAdminCustomerAlerts.Queries.GetAdminCustomerAlerts;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class AdminCustomerAlertsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/customeralerts",
        singular: "api/hospitio-admin/customeralert");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerAlertsAsync)
        .AllowAnonymous(),
        //app.MapGet($"/{Route.Singular}",GetGuestAlertAsync)
        //.AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerAlertAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerAlertAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerAlertAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetCustomerAlertsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetAdminCustomerAlertsRequest(), ct);
    }
    //private async Task<IResult> GetGuestAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    //{
    //    GetCustomerGuestAlertByIdIn @in = new() { Id = Id };
    //    return await mtrHlpr.ToResultAsync(new GetCustomerGuestAlertByIdRequest(@in), ct);
    //}
    private async Task<IResult> DeleteCustomerAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteAdminCustomerAlertsIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteAdminCustomerAlertsRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateAdminCustomerAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateAdminCustomerAlertsRequest(@in), ct);
    private async Task<IResult> UpdateCustomerAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateAdminCustomerAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateAdminCustomerAlertsRequest(@in), ct);
    #endregion
}
