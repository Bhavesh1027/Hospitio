using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;
using HospitioApi.Core.HandleMusement.Commands.CancelMusementItem;
using HospitioApi.Core.HandleMusement.Commands.DeleteMusementCartById;
using HospitioApi.Core.HandleMusement.Commands.MusementBeginPaymentStripe;
using HospitioApi.Core.HandleMusement.Commands.MusementCompletePayment;
using HospitioApi.Core.HandleMusement.Commands.MusementCreateCart;
using HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;
using HospitioApi.Core.HandleMusement.Commands.MusementLogin;
using HospitioApi.Core.HandleMusement.Commands.MusementNoPaymentFlow;
using HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIdByCartId;
using HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIds;
using HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestMusementEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-guest/musement");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Singular}/login" ,MusementLoginAsync)
           .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/musementBeginPayment" ,MusementBeginPaymentAsync)
           .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/musementNoPayment" ,MusementNoPaymentFlowAsync)
           .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/musementCompletePayment" ,MusementCompletePaymentAsync)
           .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/musementCreateOrder" ,MusementCreateOredrAsync)
           .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/GetMusementCartId" ,GetMusementCartIdAsync)
           .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/GetMusementOrderIdCartId" ,GetMusementOrderIdByCartIdAsync)
           .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/musementCreateCartId" ,MusementCreateCartIdAsync)
           .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}/deleteMusmentCartById" ,DeleteMusementCartByIdAsync)
           .AllowAnonymous(),
       app.MapGet($"/{Route.Singular}/getMusementOrderIds" ,GetMusementOrderIdsAsync)
           .AllowAnonymous(),
       app.MapPost($"/{Route.Singular}/cancelMusementItem" ,CancelMusementItemAsync)
           .AllowAnonymous(),

    };

    #region Delegates
    private async Task<IResult> MusementLoginAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new MusementLoginRequest(), ct);
    }

    private async Task<IResult> MusementBeginPaymentAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] MusementBeginPaymentIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new MusementBeginPaymentRequest(@in), ct);

    private async Task<IResult> MusementNoPaymentFlowAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] MusementNoPaymentFlowIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new MusementNoPaymentFlowRequest(@in), ct);

    private async Task<IResult> MusementCompletePaymentAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] MusementCompletePaymentIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new MusementCompletePaymentRequest(@in), ct);

    private async Task<IResult> MusementCreateOredrAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] MusementCreateOrderIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new MusementCreateOrderRequest(@in), ct);

    private async Task<IResult> GetMusementCartIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct , int GuestId)
    {
        MusementGetCartIdIn @in = new()
        {
            GuestId = GuestId
        };
        return await mtrHlpr.ToResultAsync(new MusementGetCartIdRequest(@in), ct);
    }
    private async Task<IResult> GetMusementOrderIdByCartIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string? CartId)
    {
        GetMusementOrderIdByCartIdIn @in = new()
        {
            CartId = CartId
        };
        return await mtrHlpr.ToResultAsync(new GetMusementOrderIdByCartIdRequest(@in), ct);
    }
    private async Task<IResult> MusementCreateCartIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] MusementCreateCartIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new MusementCreateCartRequest(@in), ct);

    private async Task<IResult> DeleteMusementCartByIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string? CartId)
    {
        DeleteMusementCartById @in = new()
        {
            CartId = CartId
        };
        return await mtrHlpr.ToResultAsync(new DeleteMusementCartByIdRequest(@in), ct);
    }

    private async Task<IResult> GetMusementOrderIdsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string? GuestId, string? CustomerId)
    {
        GetMusementOrderIdsIn @in = new()
        {
            GuestId = GuestId,
            CustomerId = CustomerId
        };
        return await mtrHlpr.ToResultAsync(new GetMusementOrderIdsRequest(@in), ct);
    }

    private async Task<IResult> CancelMusementItemAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CancelMusementItemIn @in, CT ct)
=> await mtrHlpr.ToResultAsync(new CancelMusementItemRequest(@in), ct);
}
    #endregion
