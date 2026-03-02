using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.ExtendCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerGuestReservation;
using HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;
using HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioMiddlewareForPMS;

public class CustomerReservationsEndPoints : IEndpointsModule
{
	public AppRoute Route { get; init; } = new(
		plural__: "api/hospitio-middleware/reservations",
		singular: "api/hospitio-middleware/reservation");

	public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
	{
		app.MapGet($"/{Route.Singular}",GetCustomerReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/create", CreateCustomerReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/createguestreservation", CreateCustomerguestReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/update", UpdateCustomerReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/updateguestreservation", UpdateCustomerguestReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/extendguestreservation", ExtendCustomerguestReservationAsync)
		.AllowAnonymous(),
		app.MapPost($"/{Route.Singular}/transferguestreservation", TransferCustomerguestReservationAsync)
		.AllowAnonymous(),
		app.MapDelete($"/{Route.Singular}", DeleteCustomerReservationAsync)
		.AllowAnonymous()
	};
	#region Delegates
	private async Task<IResult> GetCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "ReservationNumber")] string ReservationNumber, CT ct)
	{
		GetCustomerReservationByReservationNumberIn @in = new() { ReservationNumber = ReservationNumber };
		return await mtrHlpr.ToResultAsync(new GetCustomerReservationByNumberRequest(@in), ct);
	}
	private async Task<IResult> CreateCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerReservationIn @in, CT ct)
		=> await mtrHlpr.ToResultAsync(new CreateCustomerReservationRequest(@in), ct);
	private async Task<IResult> CreateCustomerguestReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestReservationIn @in, CT ct)
	=> await mtrHlpr.ToResultAsync(new CreateCustomerGuestReservationRequest(@in), ct);
	private async Task<IResult> UpdateCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerReservationIn @in, CT ct)
		=> await mtrHlpr.ToResultAsync(new UpdateCustomerReservationRequest(@in), ct);
	private async Task<IResult> UpdateCustomerguestReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestReservationIn @in, CT ct)
	=> await mtrHlpr.ToResultAsync(new UpdateCustomerGuestReservationRequest(@in), ct);
	private async Task<IResult> ExtendCustomerguestReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] ExtendCustomerGuestReservationIn @in, CT ct)
	=> await mtrHlpr.ToResultAsync(new ExtendCustomerGuestReservationRequest(@in), ct);
	private async Task<IResult> TransferCustomerguestReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] TransferCustomerGuestReservationIn @in, CT ct)
	=> await mtrHlpr.ToResultAsync(new TransferCustomerGuestReservationRequest(@in), ct);
	private async Task<IResult> DeleteCustomerReservationAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
	{
		DeleteCustomerReservationIn @in = new() { Id = Id };
		return await mtrHlpr.ToResultAsync(new DeleteCustomerReservationRequest(@in), ct);
	}
	#endregion
}
