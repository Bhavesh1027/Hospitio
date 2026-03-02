using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInGuest;
public record EditCustomerGuestPortalGuestRequest(EditCustomerGuestPortalCheckInGuestIn In) : IRequest<AppHandlerResponse>;
public class EditCustomerGuestPortalCheckInGuestHandler: IRequestHandler<EditCustomerGuestPortalGuestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public EditCustomerGuestPortalCheckInGuestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditCustomerGuestPortalGuestRequest request, CancellationToken cancellationToken)
    {
        var customerGuest = await _db.CustomerGuests.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest == null)
        {
            return _response.Error($"Customer guest could not be found.", AppStatusCodeError.Gone410);
        }

        customerGuest.AgeCategory = request.In.AgeCategory;
        customerGuest.Email = request.In.Email;
        customerGuest.Firstname = request.In.Firstname;
        customerGuest.Lastname = request.In.Lastname;
        customerGuest.PhoneCountry = request.In.PhoneCountry;
        customerGuest.PhoneNumber = request.In.PhoneNumber;

        await _db.SaveChangesAsync(cancellationToken);

        var updateCustomerGuestAlertsOut = new UpdatedCustomerGuestOut()
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerGuest.CustomerReservationId,
            ArrivalFlightNumber = customerGuest.ArrivalFlightNumber,
            BlePinCode = customerGuest.BlePinCode,
            City = customerGuest.City,
            Country = customerGuest.Country,
            DepartureAirline = customerGuest.DepartureAirline,
            DepartureFlightNumber = customerGuest.DepartureFlightNumber,
            Email = customerGuest.Email,
            FirstJourneyStep = customerGuest.FirstJourneyStep,
            Firstname = customerGuest.Firstname,
            IdProof = customerGuest.IdProof,
            IdProofNumber = customerGuest.IdProofNumber,
            IdProofType = customerGuest.IdProofType,
            Language = customerGuest.Language,
            Lastname = customerGuest.Lastname,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Picture = customerGuest.Picture,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Rating = customerGuest.Rating,
            RoomNumber = customerGuest.RoomNumber,
            Signature = customerGuest.Signature,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            TermsAccepted = customerGuest.TermsAccepted,
            DateOfBirth = customerGuest.DateOfBirth,
            Vat = customerGuest.Vat,
            IsActive = customerGuest.IsActive
        };

        return _response.Success(new EditCustomerGuestPortalCheckInGuestOut("update customer guest successful.", updateCustomerGuestAlertsOut));
    }
}
