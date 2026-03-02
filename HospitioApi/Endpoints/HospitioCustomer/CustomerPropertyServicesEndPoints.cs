using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyService;
using HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServices;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPropertyServicesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/propertyservices",
        singular: "api/hospitio-customer/propertyservice");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerPropertyServicesAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerPropertyServiceAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", CreateCustomerPropertyServiceAsync)
        .AllowAnonymous()
        //app.MapPost($"/{Route.Singular}/update", UpdateCustomerPropertyServiceAsync)
        //.AllowAnonymous(),
    };
    #region Delegates
    private async Task<IResult> GetCustomerPropertyServicesAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerPropertyInformationId")] int CustomerPropertyInformationId ,ClaimsPrincipal cp, CT ct)
    {
        GetCustomerPropertyServicesIn @in = new()
        {
            CustomerPropertyInformationId = CustomerPropertyInformationId            
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyServicesRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerPropertyServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerPropertyServiceIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerPropertyServiceRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerPropertyServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerPropertyServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerPropertyServiceRequest(@in), ct);
    //private async Task<IResult> UpdateCustomerPropertyServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerPropertyServiceIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new UpdateCustomerPropertyServiceRequest(@in), ct);
    #endregion
}
