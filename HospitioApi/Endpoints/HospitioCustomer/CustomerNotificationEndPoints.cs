using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsByCustomerId;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerNotificationEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/notifications",
        singular: "api/hospitio-customer/notification");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetNotificationAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/create",CreateNotificationAsync)
        .AllowAnonymous(),
    };
    #region Delegate 
    private async Task<IResult> GetNotificationAsync([FromServices] IMediatorHelper mtrHlpr, int PageNo, int PageSize, ClaimsPrincipal cp, CT ct)
    {
        GetNotificationsIn @in = new()
        {
            PageNo = PageNo,
            PageSize = PageSize,
            UserId = Convert.ToInt32(cp.CustomerId()),
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType"))
        };
        return await mtrHlpr.ToResultAsync(new GetNotificationsRequest(@in), ct);
    }

    private async Task<IResult> CreateNotificationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateNotificationsIn @in, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.CustomerId());
        @in.CurrentUserType = Convert.ToInt32(cp.FindFirstValue("UserType"));
        @in.UserId = Convert.ToInt32(cp.FindFirstValue("UserId"));
        return await mtrHlpr.ToResultAsync(new CreateNotificationsRequest(@in), ct);
    }
    #endregion
}

