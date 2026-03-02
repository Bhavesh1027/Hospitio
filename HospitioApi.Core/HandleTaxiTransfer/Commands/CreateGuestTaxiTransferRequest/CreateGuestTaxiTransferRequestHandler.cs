using MediatR;
using Newtonsoft.Json;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Commands.CreateGuestTaxiTransferRequest;
public record CreateGuestTaxiTransferRequestRequest(CreateGuestTaxiTransferRequestIn In) : IRequest<AppHandlerResponse>;
public class CreateGuestTaxiTransferRequestHandler : IRequestHandler<CreateGuestTaxiTransferRequestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateGuestTaxiTransferRequestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateGuestTaxiTransferRequestRequest request, CancellationToken cancellationToken)
    {
        TaxiTransferGuestRequest taxiObject = new TaxiTransferGuestRequest();
        taxiObject.CustomerId = request.In.CustomerId;
        taxiObject.GuestId = request.In.GuestId;
        taxiObject.TransferStatus = request.In.TransferStatus;
        taxiObject.TransferId = request.In.TransferId;
        taxiObject.GRPaymentId = request.In.PaymentId;
        taxiObject.GRPaymentDetails = request.In.PaymentDetails;
        taxiObject.TransferJson = request.In.TransferJson;
        taxiObject.IsActive = true;
        taxiObject.FareAmount = request.In.FareAmount;
        taxiObject.HospitioFareAmount = request.In.HospitioFareAmount;
        taxiObject.ExtraDetailsJson = JsonConvert.SerializeObject(request.In.ExtraDetails);
        taxiObject.IsMonthlyReport = true;
        taxiObject.PickUpDate = request.In.PickUpDate;


        await _db.TaxiTransferGuestRequests.AddAsync(taxiObject);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateGuestTaxiTransferRequestOut("Create guest tsxiTransfer request successful."));
    }
}   
