    using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePaymentDetails.Queries.GetAdminPaymentDetail;
using HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class PaymentDetailsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/paymentdetails",
       singular: "api/hospitio-guest/paymentdetail");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
   {
        app.MapGet($"/{Route.Singular}",GetPaymentDetailAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/adminDetail",GetAdminPaymentDetailAsync)
        .AllowAnonymous(),
    };

    private async Task<IResult> GetPaymentDetailAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "CustomerId")] int CustomerId, [FromQuery(Name = "GuestId")] int GuestId, CT ct)
    {
        GetPaymentDetailIn @in = new() { CustomerId = CustomerId, GuestId = GuestId };
        return await mtrHlpr.ToResultAsync(new GetPaymentDetailRequest(@in), ct);
    }
    private async Task<IResult> GetAdminPaymentDetailAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "CustomerId")] int? CustomerId, [FromQuery(Name = "GuestId")] int GuestId, CT ct)
    {
        GetAdminPaymentDetailIn @in = new() { CustomerId = CustomerId, GuestId = GuestId };
        return await mtrHlpr.ToResultAsync(new GetAdminPaymentDetailRequest(@in), ct);
    }
}
