using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropeetyEmergencyNumberDisplayOrder;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumbers;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPropertyEmergencyNumbersEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/property-emergency-numbers",
       singular: "api/hospitio-customer/property-emergency-number");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
         app.MapGet($"/{Route.Plural}",GetPropertyEmergencyNumbersAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetPropertyEmergencyNumberAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeletePropertyEmergencyNumberAsync)
        .AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreatePropertyEmergencyNumberAsync)
        //.AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/createupdate", UpdatePropertyEmergencyNumberAsync)
        .AllowAnonymous(),
         app.MapPost($"/{Route.Plural}/display-order", UpdateDisplayOrderAsync)
        .AllowAnonymous(),
    };

    #region Delegates
    private async Task<IResult> GetPropertyEmergencyNumbersAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "PropertyId")] int PropertyId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerPropertyEmergencyNumbersIn @in = new() { PropertyId = PropertyId };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyEmergencyNumbersRequest(@in), ct);
    }
    private async Task<IResult> GetPropertyEmergencyNumberAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerPropertyEmergencyNumberByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyEmergencyNumberByIdRequest(@in), ct);
    }
    private async Task<IResult> DeletePropertyEmergencyNumberAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerPropertyEmergencyNumberIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerPropertyEmergencyNumberRequest(@in), ct);
    }
    private async Task<IResult> UpdateDisplayOrderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdatedCustomerPropeetyEmergencyNumberDisplayOrderIn @in, CT ct)
   => await mtrHlpr.ToResultAsync(new UpdateCustomerPropeetyEmergencyNumberDisplayOrderRequest(@in), ct);
    private async Task<IResult> UpdatePropertyEmergencyNumberAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerPropertyEmergencyNumberIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerPropertyEmergencyNumberRequest(@in), ct);
    #endregion
}
