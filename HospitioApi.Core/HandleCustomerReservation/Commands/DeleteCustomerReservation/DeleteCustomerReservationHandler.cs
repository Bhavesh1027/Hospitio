using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.DeleteCustomerReservation;
public record DeleteCustomerReservationRequest(DeleteCustomerReservationIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerReservationHandler : IRequestHandler<DeleteCustomerReservationRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	public DeleteCustomerReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response)
	{
		_db = db;
		_response = response;
	}

	public async Task<AppHandlerResponse> Handle(DeleteCustomerReservationRequest request, CancellationToken cancellationToken)
	{
		var customerReservation = await _db.CustomerReservations.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
		if (customerReservation == null)
		{
			return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
		}

		_db.CustomerReservations.Remove(customerReservation);
		await _db.SaveChangesAsync(cancellationToken);

		var customerGuest = await _db.CustomerGuests.Where(c => c.CustomerReservationId == customerReservation.Id).ToListAsync(cancellationToken);
		foreach (var guest in customerGuest)
		{
			_db.CustomerGuests.Remove(guest);
			await _db.SaveChangesAsync(cancellationToken);
		}

		return _response.Success(new DeleteCustomerReservationOut("Delete customer reservation successful.", new() { Id = request.In.Id }));
	}
}
