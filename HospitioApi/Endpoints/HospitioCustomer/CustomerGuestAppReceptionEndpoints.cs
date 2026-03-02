using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppReceptionEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/reception",
       singular: "api/hospitio-customer/reception");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/get-with-relation",GetCustomerReceptionWithRelationAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerReceptionAsync)
        .AllowAnonymous(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerReceptionAsync)
        //.AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", UpdateCustomerReceptionAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/displayorderupdate" , UpdateDisplayOrderCustomerReceptionAsync).RequireAuthorization()
    };

    #region Delegates
    private async Task<IResult> GetCustomerReceptionWithRelationAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, ClaimsPrincipal cp, int AppBuilderId)
    {
        GetCustomerReceptionWithRelationIn @in = new()
        {
            AppBuilderId = AppBuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerReceptionWithRelationRequest(@in,cp.CustomerId()!, cp.UserType()!), ct);

    }

    private async Task<IResult> DeleteCustomerReceptionAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerReceptionIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerReceptionRequest(@in), ct);
    }

    //private async Task<IResult> CreateCustomerReceptionAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerReceptionIn @in, CT ct)
    //    => await mtrHlpr.ToResultAsync(new CreateCustomerReceptionRequest(@in), ct);
    private async Task<IResult> UpdateCustomerReceptionAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerReceptionIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerReceptionRequest(@in, cp.CustomerId()!), ct);

    private async Task<IResult> UpdateDisplayOrderCustomerReceptionAsync([FromServices] IMediatorHelper mtrHlpr , ClaimsPrincipal cp , [FromBody] DisplayOrderCustomerReceptionIn @in , CT ct ) 
        => await mtrHlpr.ToResultAsync(new DisplayOrderCustomerReceptionRequest(@in), ct);
    #endregion
}
