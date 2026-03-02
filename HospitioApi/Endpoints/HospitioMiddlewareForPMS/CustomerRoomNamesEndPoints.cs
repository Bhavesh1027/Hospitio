using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomByCustomerRoomGuId;
using HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomByGuIds;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioMiddlewareForPMS;

public class CustomerRoomNamesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-middleware/roomnames",
       singular: "api/hospitio-middleware/roomname");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapGet($"/{Route.Plural}",GetAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetRoomAsync)
        .AllowAnonymous(),
    };


    #region Delegates
    private async Task<IResult> GetAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] string guids, ClaimsPrincipal cp, CT ct)
    {
        var guidList = guids.Split(',').Select(Guid.Parse).ToList();
        GetCustomerRoomByGuIdsIn @in = new() { guids = guidList };
        //return await mtrHlpr.ToResultAsync(new GetCustomerByGuIdForHospitioRequest(@in), ct);
        return await mtrHlpr.ToResultAsync(new GetCustomerRoomNamesByGuIdRequest(@in), ct);
    }
    private async Task<IResult> GetRoomAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "RoomGuId")] Guid RoomGuId, [FromQuery(Name = "CustomerId")] int CustomerId, CT ct)
    {
        GetCustomerRoomByCustomerRoomGuIdIn @in = new() { guid = RoomGuId, CustomerId = CustomerId };
        return await mtrHlpr.ToResultAsync(new GetCustomerRoomNameByGuIdRequest(@in), ct);
    }
    #endregion
}
