using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGr4vy;

public class PaymentServiceDefinitionByIdEndpoint : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-gr4vy/payment-service-definitionsbyidsync");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",SyncPaymentServiceDefinitionsByIdAsync)
        .AllowAnonymous(),

    };
    #region Delegates
    private async Task<IResult> SyncPaymentServiceDefinitionsByIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new SyncPaymentServiceDefinitionsRequest(), ct);
    #endregion
}
