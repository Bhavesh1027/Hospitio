using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;
using HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequest;
using HospitioApi.Core.HandleGuestRequest.Queries.GetGuestEnhanceStayRequests;
using HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequestById;
using HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class GuestRequestEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/guestrequests",
       singular: "api/hospitio-guest/guestrequest");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapPost($"/{Route.Singular}/create", CreateGuestRequestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateGuestRequestAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetGuestRequestAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}",GetGuestRequestsAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}withenhancestay",GetGuestRequestsWithEnhanceStayAsync)
        .AllowAnonymous()
    };

    private async Task<IResult> CreateGuestRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGuestRequestIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new CreateGuestRequestRequest(@in), ct);
    private async Task<IResult> UpdateGuestRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateGuestRequestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateGuestRequestRequest(@in), ct);
    private async Task<IResult> GetGuestRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetGuestRequestByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetGuestRequestByIdRequest(@in), ct);
    }
    private async Task<IResult> GetGuestRequestsAsync([FromServices] IMediatorHelper mtrHlpr, int? PageNo, int? PageSize, string? SortColumn, string? SortOrder, ClaimsPrincipal cp, CT ct)
    {
        GetGuestRequestsIn @in = new()
        {
            SortOrder = SortOrder ?? "ASC",
            SortColumn = SortColumn ?? "TaskStatus",
            PageNo = PageNo ?? 1,
            PageSize = PageSize ?? 10
        };
        return await mtrHlpr.ToResultAsync(new GetGuestRequestsRequest(@in, Convert.ToInt32(cp.CustomerId()!)), ct);
    }
    private async Task<IResult> GetGuestRequestsWithEnhanceStayAsync([FromServices] IMediatorHelper mtrHlpr, int? CustomerId, int? GuestId, ClaimsPrincipal cp, CT ct)
    {
        GetGuestEnhanceStayRequestsIn @in = new()
        {
            CustomerId = CustomerId,
            GuestId = GuestId,
        };
        return await mtrHlpr.ToResultAsync(new GetGuestEnhanceStayRequestsRequest(@in), ct);
    }
}
