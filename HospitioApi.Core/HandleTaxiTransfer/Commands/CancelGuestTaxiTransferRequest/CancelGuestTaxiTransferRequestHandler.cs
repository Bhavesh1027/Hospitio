using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Commands.CancelGuestTaxiTransferRequest;
public record CancelGuestTaxiTransferRequestRequest(CancelGuestTaxiTransferRequestIn In) : IRequest<AppHandlerResponse>;
public class CancelGuestTaxiTransferRequestHandler : IRequestHandler<CancelGuestTaxiTransferRequestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CancelGuestTaxiTransferRequestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CancelGuestTaxiTransferRequestRequest request, CancellationToken cancellationToken)
    {

        var taxiObjectData = await _db.TaxiTransferGuestRequests.Where(s =>s.Id == request.In.Id).FirstOrDefaultAsync();
        taxiObjectData.TransferStatus = request.In.TransferStatus;
        taxiObjectData.RefundAmount = request.In.RefundAmount;
        taxiObjectData.GRPaymentId = request.In.PaymentId;
        taxiObjectData.GRPaymentDetails= request.In.PaymentDetails;
        taxiObjectData.TransferJson= request.In.TransferJson;
        taxiObjectData.RefundId= request.In.RefundId;
        taxiObjectData.RefundStatus= request.In.RefundStatus;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CancelGuestTaxiTransferRequestOut("Create guest tsxiTransfer request successful."));
    }
}
