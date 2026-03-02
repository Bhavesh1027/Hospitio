using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppHouseKeepingEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/house-keeping",
       singular: "api/hospitio-customer/house-keeping");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerHouseKeepingWithRelationAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerHouseKeepingAsync)
        .AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerHouseKeepingAsync)
        //.AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", UpdateCustomerHouseKeepingAsync)
        .RequireAuthorization(),
         app.MapPost($"/{Route.Singular}/displayorderupdate", UpdateDisplayOrderCustomerHouseKeepingAsync)
        .RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> GetCustomerHouseKeepingWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerHouseKeepingWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerHouseKeepingWithRelationRequest(@in,cp.CustomerId()!,cp.UserType()!), ct);

    }

    private async Task<IResult> DeleteCustomerHouseKeepingAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerHouseKeepingIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerHouseKeepingRequest(@in), ct);
    }

    //private async Task<IResult> CreateCustomerHouseKeepingAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerHouseKeepingIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreateCustomerHouseKeepingRequest(@in), ct);
    private async Task<IResult> UpdateCustomerHouseKeepingAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerHouseKeepingIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerHouseKeepingRequest(@in, cp.CustomerId()!), ct);

    private async Task<IResult> UpdateDisplayOrderCustomerHouseKeepingAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] DisplayOrderCustomerHouseKeepingIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new DisplayOrderCustomerHouseKeepingRequest(@in), ct);

    #endregion
}
