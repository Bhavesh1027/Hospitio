using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;
public record UpdateCustomerReservationRequest(UpdateCustomerReservationIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerReservationHandler : IRequestHandler<UpdateCustomerReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomerReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerReservationRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerReservations.Where(e => e.ReservationNumber == request.In.ReservationNumber && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer reservation already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customerReservation = await _db.CustomerReservations.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerReservation == null)
        {
            return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
        }

        customerReservation.CustomerId = request.In.CustomerId;
        customerReservation.ReservationNumber = request.In.ReservationNumber;
        customerReservation.CheckinDate = request.In.CheckinDate;
        customerReservation.CheckoutDate = request.In.CheckoutDate;
        customerReservation.NoOfGuestAdults = request.In.NoOfGuestAdults;
        customerReservation.NoOfGuestChildrens = request.In.NoOfGuestChilderns;
        customerReservation.Uuid = request.In.Uuid;
        customerReservation.Source = request.In.Source;
        customerReservation.IsActive = request.In.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        var updateCustomerGuestAlertsOut = new UpdatedCustomerReservationOut()
        {
            Id = customerReservation.Id,
            CustomerId = customerReservation.CustomerId,
            ReservationNumber = customerReservation.ReservationNumber,
            CheckinDate = customerReservation.CheckinDate,
            CheckoutDate = customerReservation.CheckoutDate,
            NoOfGuestAdults = customerReservation.NoOfGuestAdults,
            NoOfGuestChilderns = customerReservation.NoOfGuestChildrens,
            Uuid = customerReservation.Uuid,
            Source = customerReservation.Source,
            IsActive = customerReservation.IsActive
        };

        return _response.Success(new UpdateCustomerReservationOut("Update customer reservation successful.", updateCustomerGuestAlertsOut));
    }
}
