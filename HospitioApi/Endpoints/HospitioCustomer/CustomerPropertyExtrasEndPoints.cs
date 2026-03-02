using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropeetyEmergencyNumberDisplayOrder;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtras;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.NewFolder;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
using HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraById;
using HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtras;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPropertyExtrasEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
  plural__: "api/hospitio-customer/property-extras",
  singular: "api/hospitio-customer/property-extra");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetPropertyExtrasAsync)
    .AllowAnonymous(),
    app.MapGet($"/{Route.Singular}",GetPropertyExtraAsync)
    .AllowAnonymous(),
    app.MapDelete($"/{Route.Singular}", DeletePropertyExtraAsync)
    .AllowAnonymous(),
    //app.MapPost($"/{Route.Singular}/create", CreatePropertyExtraAsync)
    //.AllowAnonymous(),
    app.MapPost($"/{Route.Singular}/createupdate", UpdatePropertyExtraAsync)
    .AllowAnonymous(),
      app.MapPost($"/{Route.Plural}/display-order", UpdateDisplayOrderAsync)
    .AllowAnonymous(),
    app.MapDelete($"/{Route.Singular}details", DeletePropertyExtraDetailAsync)
    .AllowAnonymous(),
};

    #region Delegates
    private async Task<IResult> GetPropertyExtrasAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerPropertyInformationId")] int CustomerPropertyInformationId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerPropertyExtrasIn @in = new() { CustomerPropertyInformationId = CustomerPropertyInformationId };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyExtrasRequest(@in), ct);
    }
    private async Task<IResult> GetPropertyExtraAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerPropertyExtraByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyExtraByIdRequest(@in), ct);
    }
    private async Task<IResult> DeletePropertyExtraAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerPropertyExtrasIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerPropertyExtrasRequest(@in), ct);
    }
    //private async Task<IResult> CreatePropertyExtraAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerPropertyExtrasIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreateCustomerPropertyExtrasRequest(@in), ct);
    private async Task<IResult> UpdatePropertyExtraAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerPropertyExtrasIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerPropertyExtrasRequest(@in), ct);

    private async Task<IResult> UpdateDisplayOrderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdatedCustomerPropertyExtraDisplayOrderIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new UpdateCustomerPropertyExtraDisplayOrderRequest(@in), ct);
    private async Task<IResult> DeletePropertyExtraDetailAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerPropertyExtraDetailIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerPropertyExtraDetailRequest(@in), ct);
    }
    #endregion
}
