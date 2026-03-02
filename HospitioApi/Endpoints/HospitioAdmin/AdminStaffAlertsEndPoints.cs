using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.CreateAdminStaffAlerts;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlerts;
using HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;
using HospitioApi.Core.HandleAdminStaffAlerts.Queries.GetAdminStaffAlerts;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class AdminStaffAlertsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-admin/staff-alerts",
    singular: "api/hospitio-admin/staff-alert");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetStaffAlertsAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateStaffAlertAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteStaffAlertAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateStaffAlertAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetStaffAlertsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetAdminStaffAlertsRequest(), ct);
    }
    private async Task<IResult> CreateStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateAdminStaffAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateAdminStaffAlertsRequest(@in), ct);
    private async Task<IResult> DeleteStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteAdminStaffAlertsIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteAdminStaffAlertsRequest(@in), ct);
    }
    private async Task<IResult> UpdateStaffAlertAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateAdminStaffAlertsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateAdminStaffAlertsRequest(@in), ct);
    #endregion
}
