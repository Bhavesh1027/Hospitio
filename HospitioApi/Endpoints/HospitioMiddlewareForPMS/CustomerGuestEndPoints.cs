using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioMiddlewareForPMS;

public class CustomerGuestEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-middleware/guests",
        singular: "api/hospitio-middleware/guest");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",GetCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerGuestAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "ReservationId")] int ReservationId, CT ct)
    {
        GetMainGuestByReservationIdIn @in = new() { ReservationId = ReservationId };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestByReservationIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerGuestIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerGuestRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "CustomerId")] int CustomerId, [FromBody] CreateCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerGuestRequest(@in, CustomerId.ToString()), ct);
    private async Task<IResult> UpdateCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerGuestRequest(@in), ct);
    #endregion
}
