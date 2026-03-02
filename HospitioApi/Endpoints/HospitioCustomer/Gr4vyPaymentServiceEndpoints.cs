using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.CreateCustomerGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.UpdateCustomerGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.VerifyCustomerGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentService;
using HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentServiceById;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class Gr4vyPaymentServiceEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-customer/PaymentServices",
      singular: "api/hospitio-customer/PaymentService");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapPost($"/{Route.Singular}Create",AddCustomerGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapPut($"/{Route.Singular}Update",UpdateCustomerGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapPost($"/{Route.Singular}Verify",VerifyCustomerGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
         app.MapGet($"/{Route.Singular}",GetCustomerGr4vyPaymentServiceByIdAsync)
        .RequireAuthorization(),
         app.MapGet($"/{Route.Plural}",GetCustomerGr4vyPaymentServiceAsync)
        .RequireAuthorization(),
    };
    #region Delegates
    private async Task<IResult> AddCustomerGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomerGr4vyPaymentServiceRequest(@in), ct);
    }
    private async Task<IResult> UpdateCustomerGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomerGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new UpdateCustomerGr4vyPaymentServiceRequest(@in), ct);
    }
    private async Task<IResult> VerifyCustomerGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] VerifyCustomerGr4vyPaymentServiceIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new VerifyCustomerGr4vyPaymentServiceRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGr4vyPaymentServiceByIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "CustomerPaymentProcessorId")] int CustomerPaymentProcessorId, CT ct)
    {
        GetCustomerGr4vyPaymentServiceByIdIn @in = new() { CustomerPaymentProcessorId = CustomerPaymentProcessorId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGr4vyPaymentServiceByIdHandlerRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGr4vyPaymentServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGr4vyPaymentServiceIn @in = new()
        {
            CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGr4vyPaymentServiceRequest(@in), ct);
    }
   
    #endregion
}
