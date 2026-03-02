using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetail;
using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationById;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerReservationsEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/reservations",
        singular: "api/hospitio-customer/reservation");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerReservationsAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerReservationAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerReservationAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerReservationAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerReservationAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/createwithguestdetail", CreateCustomerGuestReservationAsync)
        .RequireAuthorization(),
    };
    #region Delegates
    private async Task<IResult> GetCustomerReservationsAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerId")] int CustomerId, string? SearchColumn, string? SearchValue, int? PageNo, int? PageSize, string? SortColumn, string? SortOrder, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerReservationsIn @in = new()
        {
            CustomerId = CustomerId,
            SortOrder = SortOrder ?? "ASC",
            SortColumn = SortColumn ?? "ReservationNumber",
            SearchColumn = SearchColumn ?? string.Empty,
            SearchValue = SearchValue ?? string.Empty,
            PageNo = PageNo ?? 1,
            PageSize = PageSize ?? 10,
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerReservationsRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerReservationByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerReservationByIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerReservationIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerReservationRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerReservationIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerReservationRequest(@in), ct);
    private async Task<IResult> UpdateCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerReservationIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerReservationRequest(@in), ct);
    private async Task<IResult> CreateCustomerGuestReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerAutoReservationWithGestDetailIn @in, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomerAutoReservationWithGestDetailRequest(@in), ct);
    }

    #endregion
}
