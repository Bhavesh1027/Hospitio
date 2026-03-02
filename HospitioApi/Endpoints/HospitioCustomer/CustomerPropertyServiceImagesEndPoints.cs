using MediatR;
using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.CreateCustomerPropertyServiceImage;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImage;
using HospitioApi.Core.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImages;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerPropertyServiceImageImagesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/propertyserviceimages",
        singular: "api/hospitio-customer/propertyserviceimage");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerPropertyServiceImagesAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerPropertyServiceImageAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerPropertyServiceImageAsync)
        .AllowAnonymous(),
    };
    #region Delegates
    private async Task<IResult> GetCustomerPropertyServiceImagesAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerPropertyServiceId")] int CustomerPropertyServiceId, string? SearchColumn, string? SearchValue, int? PageNo, int? PageSize, string? SortColumn, string? SortOrder, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerPropertyServiceImagesIn @in = new()
        {
            CustomerPropertyServiceId = CustomerPropertyServiceId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerPropertyServiceImagesRequest(@in), ct);
    }

    private async Task<IResult> CreateCustomerPropertyServiceImageAsync(IMediator _mediator, HttpRequest request, ClaimsPrincipal cp, int CustomerPropertyServiceId, bool? IsActive, CT ct)
    {
        if (!request.HasFormContentType)
            return Results.NoContent();

        var form = await request.ReadFormAsync(ct);
        var formFile = form.Files["file"];

        if (formFile is null || formFile.Length == 0)
            return Results.BadRequest();

        CreateCustomerPropertyServiceImageIn @in = new();
        @in.CustomerPropertyServiceId = CustomerPropertyServiceId;
        @in.IsActive = IsActive;

        var handlerResponse = await _mediator.Send(new CreateCustomerPropertyServiceImageRequest(@in, form.Files[0]), ct);

        var response = (CreateCustomerPropertyServiceImageOut)(handlerResponse.Response!);

        return handlerResponse.HasResponse
            ? Results.Ok(response.Message)
            : Results.NotFound();
    }

    private async Task<IResult> DeleteCustomerPropertyServiceImageAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerPropertyServiceImageIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerPropertyServiceImageRequest(@in), ct);
    }
    #endregion
}
