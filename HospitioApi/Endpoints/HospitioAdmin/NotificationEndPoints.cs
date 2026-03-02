using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class NotificationEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/notifications",
        singular: "api/hospitio-admin/notification");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetNotificationAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/create",CreateNotificationAsync)
        .AllowAnonymous()
    };
    #region Delegate 
    private async Task<IResult> GetNotificationAsync([FromServices] IMediatorHelper mtrHlpr, int PageNo, int PageSize, ClaimsPrincipal cp, CT ct)
    {
        GetNotificationsIn @in = new()
        {
            PageNo = PageNo,
            PageSize = PageSize,
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType"))
        };
        return await mtrHlpr.ToResultAsync(new GetNotificationsRequest(@in), ct);
    }
    private async Task<IResult> CreateNotificationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateNotificationsIn @in, CT ct)
    {
        @in.CurrentUserType = Convert.ToInt32(cp.FindFirstValue("UserType"));
        return await mtrHlpr.ToResultAsync(new CreateNotificationsRequest(@in), ct);
    }
    #endregion
}
