using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConcierge;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConciergeItem;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
using HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppConciergeEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/concierge",
       singular: "api/hospitio-customer/concierge");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerConciergeWithRelationAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerConciergeAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}item", DeleteCustomerConciergeItemAsync)
        .RequireAuthorization(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerConciergeAsync)
        //.AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", UpdateCustomerConciergeAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/displayorderupdate", UpdateDisplayOrderCustomerConciergeAsync)
        .RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> GetCustomerConciergeWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, int AppBuilderId)
    {
        GetCustomerConciergeWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerConciergeWithRelationRequest(@in,cp.CustomerId()!,cp.UserType()!), ct);

    }

    private async Task<IResult> DeleteCustomerConciergeAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerConciergeIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerConciergeRequest(@in), ct);
    }

    private async Task<IResult> DeleteCustomerConciergeItemAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerConciergeItemIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerConciergeItemRequest(@in), ct);
    }

    //private async Task<IResult> CreateCustomerConciergeAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerConciergeIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreateCustomerConciergeRequest(@in), ct);
    private async Task<IResult> UpdateCustomerConciergeAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerConciergeIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerConciergeRequest(@in, cp.CustomerId()!), ct);
    private async Task<IResult> UpdateDisplayOrderCustomerConciergeAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] DisplayOrderCustomerConcierageIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new DisplayOrderCustomerConcierageRequest(@in), ct);
    #endregion
}
