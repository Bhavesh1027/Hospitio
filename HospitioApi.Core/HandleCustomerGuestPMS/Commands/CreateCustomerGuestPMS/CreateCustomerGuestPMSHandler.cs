using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;
public record CreateCustomerGuestPMSRequest(CreateCustomerGuestPMSIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class CreateCustomerGuestPMSHandler : IRequestHandler<CreateCustomerGuestPMSRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    public CreateCustomerGuestPMSHandler(ApplicationDbContext db, IUserFilesService userFilesService, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestPMSRequest request, CancellationToken cancellationToken)
    {

        string filePath = "";
        //var webFile = new WebFileOut();
        if (request.In.ContainerName is null)
            filePath = request.In.DocumentType!;
        else
            filePath = request.In.ContainerName + "\\" + request.In.DocumentType;
        //if (request.In.File is not null)
        //{
        var webFile = await _userFilesService.UploadWebFileOnGivenPathAsync(request.In.DocumentAttachment!, filePath, cancellationToken, false);
        //}
        if (webFile is null || webFile.MemoryStream is null || webFile.MemoryStream.Length <= 0)
        {
            return _response.Error("Unable to uploaded documentattachment.", AppStatusCodeError.InternalServerError500);
        }
        var reservation = new CustomerReservation()
        {
            CustomerId = Convert.ToInt32(request.CustomerId),
            ReservationNumber = request.In.ReservationNumber,
            CheckinDate = request.In.ArrivalDate,
            CheckoutDate = request.In.DepartureDate,
            IsActive = true
        };
        await _db.CustomerReservations.AddAsync(reservation, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var guest = new CustomerGuest()
        {
            CustomerReservationId = reservation.Id,
            Firstname = request.In.FirstName,
            Lastname = request.In.LastName,
            Email = request.In.Email,
            PhoneNumber = request.In.MobileNumber,
            Street = request.In.Street,
            Postalcode = request.In.PostalCode,
            City = request.In.City,
            Country = request.In.Country,
            Vat = request.In.VATNumber,
            IdProofType = request.In.DocumentName,
            IdProof = webFile.Name,
            IsActive = true
        };
        await _db.CustomerGuests.AddAsync(guest, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        webFile.MemoryStream.Position = 0;
        return _response.Success(new CreateCustomerGuestPMSOut("Guest create successful.", new()
        {
            FirstName = guest.Firstname,
            LastName = guest.Lastname,
            Email = guest.Email,
            DocumentName = guest.IdProofType,
            FileName = webFile.Name            
        }));

    }
}
