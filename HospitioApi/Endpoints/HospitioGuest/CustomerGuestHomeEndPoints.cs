using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;
using HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestHomeEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-guest/home");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapGet($"/{Route.Singular}",GetCustomerHomeAsync)
        .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetCustomerHomeAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int RoomId, int CustomerId)
    {
        GetCustomerGuestAppBuilderByCustomerRoomIdIn @in = new()
        {
            RoomId = RoomId,
            CustomerId = CustomerId,
            UserType = (UserTypeEnum.Guest).ToString()

        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAppBuilderByCustomerRoomIdRequest(@in), ct);
    }
    #endregion
}
