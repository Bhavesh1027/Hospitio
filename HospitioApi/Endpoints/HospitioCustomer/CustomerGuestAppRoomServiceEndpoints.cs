using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppRoomServiceEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/roomservice",
       singular: "api/hospitio-customer/roomservice");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerRoomServiceWithRelationAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerRoomServiceAsync)
        .AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerRoomServiceAsync)
        //.AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", UpdateCustomerRoomServiceAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/displayorderupdate", UpdateDisplayOrderCustomerRoomServiceAsync)
        .RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> GetCustomerRoomServiceWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerRoomServiceWithRelationIn @in = new()
        {
           AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerRoomServiceWithRelationRequest(@in,cp.CustomerId()!, cp.UserType()!), ct);

    }

    private async Task<IResult> DeleteCustomerRoomServiceAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerRoomServiceIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerRoomServiceRequest(@in), ct);
    }

    private async Task<IResult> CreateCustomerRoomServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerRoomServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerRoomServiceRequest(@in), ct);
    private async Task<IResult> UpdateCustomerRoomServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerRoomServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerRoomServiceRequest(@in,cp.CustomerId()!), ct);
    #endregion

    private async Task<IResult> UpdateDisplayOrderCustomerRoomServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] DisplayOrderCustomerRoomServiceIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new DisplayOrderCustomerRoomServiceRequest(@in), ct);
}