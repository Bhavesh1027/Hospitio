using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTransactions.Commands.CaptureAdminTransaction;
using HospitioApi.Core.HandleTransactions.Commands.CaptureTransaction;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CaptureTransactionsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-guest/transactions",
      singular: "api/hospitio-guest/transaction");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapPost($"/{Route.Singular}/capture", CaptureTransactionAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/adminCapture", CaptureAdminTransactionAsync)
        .AllowAnonymous()
    };

    #region Delegates

    private async Task<IResult> CaptureTransactionAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CaptureTransactionIn @in, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new CaptureTransactionRequest(@in), ct);
    }

    private async Task<IResult> CaptureAdminTransactionAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CaptureAdminTransactionIn @in, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new CaptureAdminTransactionRequest(@in), ct);
    }
    #endregion
}
