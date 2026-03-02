using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTaxiTransfer.Commands.CancelGuestTaxiTransferRequest;
using HospitioApi.Core.HandleTaxiTransfer.Commands.CreateGuestTaxiTransferRequest;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetTransferDataByGuestId;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class GuestTaxiTransferRequest : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/taxiTransferRequests",
       singular: "api/hospitio-guest/taxiTransferRequest");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapPost($"/{Route.Singular}/create", CreateGuestTaxiTransferRequestAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/getTransferDatabyGuestId", GetTransferDatabyGuestId)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/cancel", CancelGuestTaxiTransferRequestAsync)
        .AllowAnonymous(),
    };

    private async Task<IResult> CreateGuestTaxiTransferRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGuestTaxiTransferRequestIn @in, CT ct)
   => await mtrHlpr.ToResultAsync(new CreateGuestTaxiTransferRequestRequest(@in), ct);
    private async Task<IResult> GetTransferDatabyGuestId([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string? GuestId, string? CustomerId)
    {
        GetTransferDataByGuestIdIn @in = new()
        {
            GuestId = GuestId,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetTransferDataByGuestRequest(@in), ct);
    }
    private async Task<IResult> CancelGuestTaxiTransferRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CancelGuestTaxiTransferRequestIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new CancelGuestTaxiTransferRequestRequest(@in), ct);

}
