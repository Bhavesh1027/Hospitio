using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.CreateCustomersPaymentProcessors;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessors;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.UpdateCustomersPaymentProcessors;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsUsingCustomerId;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomersPaymentProcessorsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/payment-processors",
        singular: "api/hospitio-customer/payment-processor");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetPaymentProcessorsAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Plural}/bycustomerid  ",GetPaymentProcessorsByCustomerIdAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetPaymentProcessorAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeletePaymentProcessorAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreatePaymentProcessorAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update ", UpdatePaymentProcessorAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetPaymentProcessorsAsync([FromServices] IMediatorHelper mtrHlpr, int CustomerId, int? PageNo, int? PageSize, CT ct)
    {
        GetCustomersPaymentProcessorsIn @in = new()
        {
            PageNo = PageNo ?? 1,
            PageSize = PageSize ?? 10,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersPaymentProcessorsRequest(@in), ct);
    }
    private async Task<IResult> GetPaymentProcessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomersPaymentProcessorsByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomersPaymentProcessorsByIdRequest(@in), ct);
    }

    private async Task<IResult> GetPaymentProcessorsByCustomerIdAsync([FromServices] IMediatorHelper mtrHlpr, CT ct,ClaimsPrincipal cp
        )
    {
        GetCustomersPaymentProcessorsByCustomerIdIn @in = new()
        {
            CustomerId = Convert.ToInt32(cp.CustomerId())
        };
        return await mtrHlpr.ToResultAsync(new GetCustomersPaymentProcessorsByCustomerIdRequest (@in), ct);
    }
    private async Task<IResult> DeletePaymentProcessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomersPaymentProcessorsIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomersPaymentProcessorsRequest(@in), ct);
    }
    private async Task<IResult> CreatePaymentProcessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomersPaymentProcessorsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomersPaymentProcessorsRequest(@in), ct);
    private async Task<IResult> UpdatePaymentProcessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomersPaymentProcessorsIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomersPaymentProcessorsRequest(@in), ct);
    #endregion
}
