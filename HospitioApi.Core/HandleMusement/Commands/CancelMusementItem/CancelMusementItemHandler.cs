using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Commands.CancelMusementItem;
public record CancelMusementItemRequest(CancelMusementItemIn In) : IRequest<AppHandlerResponse>;
public class CancelMusementItemHandler : IRequestHandler<CancelMusementItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CancelMusementItemHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CancelMusementItemRequest request, CancellationToken cancellationToken)
    {

        var musementItemInfo = await _db.MusementItemInfos.Where(s => s.ItemUUID == request.In.ItemUUID && s.DeletedAt == null && s.IsCancel == false).FirstOrDefaultAsync(cancellationToken);

        if (musementItemInfo == null)
        {
            return _response.Error("Item Does not exits", AppStatusCodeError.Gone410);
        }

        var MusementPaymentInfo = await _db.MusementPaymentInfos.Where(s => s.OrderUUID == request.In.OrderUUID).FirstOrDefaultAsync(cancellationToken);

        if(MusementPaymentInfo != null)
        {
            MusementPaymentInfo.Amount = MusementPaymentInfo.Amount - musementItemInfo.TotalPrice;
        }

        musementItemInfo.IsCancel = true;
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new CancelMusementItemOut("Item Cancel successful."));
    }
}
