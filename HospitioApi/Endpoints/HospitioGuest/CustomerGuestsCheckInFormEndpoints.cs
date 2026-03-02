using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilder;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInGuest;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservation;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestCheckInFormBuilderIcon;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestsByReservation;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
using HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.SendGuestPdfMail;
using HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class CustomerGuestsCheckInFormEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-guest/guest-checkIns",
        singular: "api/hospitio-guest/guest-checkIn");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}formfields",GetCustomerGuestsCheckInFormBuilderAsync)
        .AllowAnonymous(),  // Clear   This is for getting require fields for form fill up
          app.MapGet($"/{Route.Singular}/icon",GetCustomerGuestsCheckInFormBuilderIconAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}guestbyid",GetCustomerGuestAsync)
        .AllowAnonymous(),  //This is for customer guest get by id
        app.MapGet($"/{Route.Singular}reservation",GetCustomerGuestsCheckInReservationAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}usingreservation",GetCustomerGuestsByReservationAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/createguest", CreateCustomerGuestCheckInFormBuilderAsync)
        .AllowAnonymous(), //Clear Add guest and return link
        app.MapPost($"/{Route.Singular}/updatereservation", EditCustomerGuestCheckInFormBuilderAsync)
        .AllowAnonymous(), // Clear Enter guests numbers
        app.MapPost($"/{Route.Singular}/updateguest", UpdateCustomerGuestAsync)
        .AllowAnonymous(), // Clear Update co-Guest
        app.MapPost($"/{Route.Singular}/updatemainguest", UpdateMainCustomerGuestAsync)
        .AllowAnonymous(), // Not only main all guest checdk in detail
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/sendGuestPdfMail", SendGuestPdfMailAsync)
        .AllowAnonymous(),
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestsCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerId")] int CustomerId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestsCheckInFormBuilderIn @in = new() { CustomerId = CustomerId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestsCheckInFormBuilderRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGuestsCheckInFormBuilderIconAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerId")] int CustomerId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestCheckInFormBuilderIconIn @in = new() { CustomerId = CustomerId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestPortalCheckInFormBuilderIconRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGuestsCheckInReservationAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "GuestId")] int GuestId, [FromQuery(Name = "ReservationId")] int ReservationId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestPortalCheckInFormBuilderIn @in = new() { GuestId = GuestId, ReservationId = ReservationId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestPortalCheckInFormBuilderRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestPortalCheckInFormBuilderIn @in, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new CreateCustomerGuestPortalCheckInFormBuilderRequest(@in), ct);
    }
    private async Task<IResult> EditCustomerGuestCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] EditCustomerGuestPortalCheckInReservationIn @in, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new EditGuestAppCustomerReservationRequest(@in), ct);
    }
    private async Task<IResult> UpdateCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] EditCustomerGuestPortalCheckInGuestIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new EditCustomerGuestPortalGuestRequest(@in), ct);
    private async Task<IResult> DeleteCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int GuestId, CT ct)
    {
        DeleteCustomerGuestIn @in = new() { Id = GuestId };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerGuestRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "GuestId")] int GuestId, CT ct)
    {
        GetCustomerGuestByIdIn @in = new() { Id = GuestId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestByIdRequest(@in), ct);
    }
    private async Task<IResult> UpdateMainCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerGuestRequest(@in), ct);
    private async Task<IResult> GetCustomerGuestsByReservationAsync([FromServices] IMediatorHelper mtrHlpr, int ReservationId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestsByReservationIn @in = new()
        {
            ReservationId = ReservationId,
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestsByReservationRequest(@in), ct);
    }

    private async Task<IResult> SendGuestPdfMailAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] SendGuestPdfMailIn @in, CT ct)
          => await mtrHlpr.ToResultAsync(new SendGuestPdfMailRequest(@in), ct);
    #endregion
}
