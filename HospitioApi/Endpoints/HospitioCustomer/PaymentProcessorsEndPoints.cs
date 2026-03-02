using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessors;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class PaymentProcessorsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/paymentprocessors",
        singular: "api/hospitio-customer/paymentprocessor");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetPaymentprocessorsAsync)
        .RequireAuthorization(),
        //app.MapGet($"/{Route.Singular}",GetPaymentprocessorAsync)
        //.AllowAnonymous(),
        //app.MapDelete($"/{Route.Singular}", DeletePaymentprocessorAsync)
        //.AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreatePaymentprocessorAsync)
        //.AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/update", UpdatePaymentprocessorAsync)
        //.AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetPaymentprocessorsAsync([FromServices] IMediatorHelper mtrHlpr, CT ct)
        => await mtrHlpr.ToResultAsync(new GetPaymentProcessorsRequest(), ct);
    //private async Task<IResult> GetPaymentprocessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    //{
    //    GetPaymentProcessorsByIdIn @in = new() { Id = Id };
    //    return await mtrHlpr.ToResultAsync(new GetPaymentProcessorsByIdRequest(@in), ct);
    //}
    //private async Task<IResult> DeletePaymentprocessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    //{
    //    DeletePaymentProcessorsIn @in = new() { Id = Id };
    //    return await mtrHlpr.ToResultAsync(new DeletePaymentProcessorsRequest(@in), ct);
    //}
    //private async Task<IResult> CreatePaymentprocessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreatePaymentProcessorsIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreatePaymentProcessorsRequest(@in), ct);
    //private async Task<IResult> UpdatePaymentprocessorAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdatePaymentProcessorsIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new UpdatePaymentProcessorsRequest(@in), ct);
    #endregion
}
