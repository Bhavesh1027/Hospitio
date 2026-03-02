using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandleNotifications.Queries.GetNotifications;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestNotificationEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-guest/notifications",
        singular: "api/hospitio-guest/notification");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetNotificationAsync)
        .AllowAnonymous(),
    };
    #region Delegate 
    private async Task<IResult> GetNotificationAsync([FromServices] IMediatorHelper mtrHlpr, int PageNo, int PageSize, int GuestId ,ClaimsPrincipal cp, CT ct)
    {
        GetNotificationsIn @in = new()
        {
            PageNo = PageNo,
            PageSize = PageSize,
            UserId = GuestId,
            UserType = (int)UserTypeEnum.Guest
        };
        return await mtrHlpr.ToResultAsync(new GetNotificationsRequest(@in), ct);
    }

    #endregion
}
