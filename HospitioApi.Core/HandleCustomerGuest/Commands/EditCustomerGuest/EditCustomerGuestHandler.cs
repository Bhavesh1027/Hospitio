using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.EditCustomerGuest;
public record EditCustomerGuestRequest(EditCustomerGuestIn In) : IRequest<AppHandlerResponse>;
public class EditCustomerGuestHandler : IRequestHandler<EditCustomerGuestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public EditCustomerGuestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditCustomerGuestRequest request, CancellationToken cancellationToken)
    {
        var customerGuest = await _db.CustomerGuests.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest == null)
        {
            return _response.Error($"Customer guest could not be found.", AppStatusCodeError.Gone410);
        }

        customerGuest.CustomerReservationId = request.In.CustomerReservationId;
        customerGuest.Firstname = request.In.Firstname;
        customerGuest.Lastname = request.In.Lastname;
        customerGuest.Email = request.In.Email;
        customerGuest.PhoneCountry = request.In.PhoneCountry;
        customerGuest.PhoneNumber = request.In.PhoneNumber;
        customerGuest.Country = request.In.Country;
        customerGuest.City = request.In.City;
        customerGuest.Pin = request.In.Pin;
        customerGuest.Postalcode = request.In.Postalcode;
        customerGuest.Street = request.In.Street;
        customerGuest.StreetNumber = request.In.StreetNumber;
        customerGuest.BlePinCode = request.In.BlePinCode;

        var guestReservation = await _db.CustomerReservations.Where(e => e.Id == request.In.CustomerReservationId).FirstOrDefaultAsync(cancellationToken);

        guestReservation.CheckinDate = request.In.CheckinDate;
        guestReservation.CheckoutDate = request.In.CheckoutDate;

        await _db.SaveChangesAsync(cancellationToken);

        var editedCustomerGuestOut = new EditedCustomerGuestOut()
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerGuest.CustomerReservationId,
            FirstName = customerGuest.Firstname,
            LastName = customerGuest.Lastname,
            Email = customerGuest.Email,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Country = customerGuest.Country,
            City = customerGuest.City,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            BlePinCode = customerGuest.BlePinCode,
            CheckinDate = guestReservation.CheckinDate,
            CheckoutDate = guestReservation.CheckoutDate,
            CreatedAt = customerGuest.CreatedAt,
            IsActive = customerGuest.IsActive,
        };

        return _response.Success(new EditCustomerGuestOut("update customer guest successful.", editedCustomerGuestOut));
    }
}

