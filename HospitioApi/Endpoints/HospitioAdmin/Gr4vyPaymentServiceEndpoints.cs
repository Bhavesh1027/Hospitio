using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGr4vyPaymentService.Commands.CreateGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentService.Commands.UpdateGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentService.Commands.VerifyGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentServiceById;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class Gr4vyPaymentServiceEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-admin/PaymentServices",
      singular: "api/hospitio-admin/PaymentService");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapPost($"/{Route.Singular}Create",AddGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapPut($"/{Route.Singular}Update",UpdateGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapPost($"/{Route.Singular}Verify",VerifyGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapGet($"/{Route.Singular}",GetGr4vyPaymentServiceByIdAsync)
        .RequireAuthorization(),
         app.MapGet($"/{Route.Plural}",GetGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
    };
    #region Delegates
    private async Task<IResult> AddGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
            => await mtrHlpr.ToResultAsync(new CreateGr4vyPaymentServiceRequest(@in), ct);
    private async Task<IResult> UpdateGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
            => await mtrHlpr.ToResultAsync(new UpdateGr4vyPaymentServiceRequest(@in), ct);
    private async Task<IResult> VerifyGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] VerifyGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
            => await mtrHlpr.ToResultAsync(new VerifyGr4vyPaymentServiceRequest(@in), ct);
    private async Task<IResult> GetGr4vyPaymentServiceByIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "HospitioPaymentProcessorId")] int HospitioPaymentProcessorId, CT ct)
    {
        GetGr4vyPaymentServiceByIdIn @in = new() { HospitioPaymentProcessorId = HospitioPaymentProcessorId };
        return await mtrHlpr.ToResultAsync(new GetGr4vyPaymentServiceByIdHandlerRequest(@in), ct);
    }
    private async Task<IResult> GetGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    => await mtrHlpr.ToResultAsync(new GetGr4vyPaymentServiceRequest(), ct);
    #endregion
}
