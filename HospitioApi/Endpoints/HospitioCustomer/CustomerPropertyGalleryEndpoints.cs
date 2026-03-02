using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;
using HospitioApi.Core.HandlePropertyGallery.Commands.EditPropertyGallery;
using HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyFile;
using HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;
public class CustomerPropertyGalleryEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        singular: "api/hospitio-customer/property-gallery");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}" +"/PropertyInformationId",GetPropertyGalleryAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}" + "/{FileId}/{CustomerPropertyInformationId}" ,GetPropertyFileAsync)
        .AllowAnonymous(),

        app.MapPost($"/{Route.Singular}/create", CreateCustomerPropertyGalleryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/edit", EditCustomerPropertyGalleryAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetPropertyGalleryAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "PropertyInformationId")] int CustomerPropertyInformationId, ClaimsPrincipal cp, CT ct)
    {
        GetPropertyGalleryIn @in = new() { CustomerPropertyInformationId = CustomerPropertyInformationId };
        return await mtrHlpr.ToResultAsync(new GetPropertyGalleryRequest(@in), ct);
    }
    private async Task<IResult> GetPropertyFileAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "FileId")] int Id, [FromQuery(Name = "CustomerPropertyInformationId")] int CustomerPropertyInformationId, ClaimsPrincipal cp, CT ct)
    {
        GetPropertyFileIn @in = new() { Id = Id, CustomerPropertyInformationId = CustomerPropertyInformationId };
        return await mtrHlpr.ToResultAsync(new GetPropertyFileRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerPropertyGalleryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreatePropertyGalleryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreatePropertyGalleryRequest(@in), ct);
    private async Task<IResult> EditCustomerPropertyGalleryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] EditPropertyGalleryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditPropertyGalleryRequest(@in), ct);
    #endregion
}
