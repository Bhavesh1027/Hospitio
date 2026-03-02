using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorByPaymentProcessorsId;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class PaymentProcessorsDefinationsEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/PaymentProcessorsDefinations",
        singular: "api/hospitio-customer/PaymentProcessorsDefination");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {

        app.MapGet($"/{Route.Singular}",GetPaymentProcessorsDefinationByPaymentProcessorsIdAsync)
        .RequireAuthorization()

    };
    #region Delegates


    private async Task<IResult> GetPaymentProcessorsDefinationByPaymentProcessorsIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int PaymentProcessorId, CT ct)
    {
        GetPaymentProcessorsDefinationsByPaymentProcessorsIdIn @in = new() { PaymentProcessorId = PaymentProcessorId };
        return await mtrHlpr.ToResultAsync(new GetPaymentProcessorsDefinationsByPaymentProcessorsIdRequest(@in), ct);
    }
    #endregion
}
