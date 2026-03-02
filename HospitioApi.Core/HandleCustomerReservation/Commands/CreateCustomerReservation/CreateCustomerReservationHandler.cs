using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerReservation;

public record CreateCustomerReservationRequest(CreateCustomerReservationIn In) : IRequest<AppHandlerResponse>;

public class CreateCustomerReservationHandler : IRequestHandler<CreateCustomerReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateCustomerReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerReservationRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerReservations.Where(e => e.ReservationNumber == request.In.ReservationNumber).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer reservation already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var customerReservation = new CustomerReservation
        {
            CustomerId = request.In.CustomerId,
            ReservationNumber = request.In.ReservationNumber,
            CheckinDate = request.In.CheckinDate,
            CheckoutDate = request.In.CheckoutDate,
            NoOfGuestAdults = request.In.NoOfGuestAdults,
            NoOfGuestChildrens = request.In.NoOfGuestChilderns,
            Uuid = request.In.Uuid,
            Source = request.In.Source,
            IsActive = request.In.IsActive,
        };

        await _db.CustomerReservations.AddAsync(customerReservation);
        await _db.SaveChangesAsync(cancellationToken);
        var createdCustomerReservationOut = new CreatedCustomerReservationOut
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

        return _response.Success(new CreateCustomerReservationOut("Create customer reservation successful.", createdCustomerReservationOut));
    }
}
