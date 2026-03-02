using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Commands.MusementCreateCart;
public record MusementCreateCartRequest(MusementCreateCartIn In) : IRequest<AppHandlerResponse>;
public class MusementCreateCartHandler : IRequestHandler<MusementCreateCartRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public MusementCreateCartHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(MusementCreateCartRequest request, CancellationToken cancellationToken)
    {

        var musementCartId = await _db.MusementGuestInfos.Where(s => s.CartUUID == request.In.cartid && s.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

        if (musementCartId != null)
        {
            return _response.Error("Cart Id alredy exits", AppStatusCodeError.Gone410);
        }
        var MusementCartInfo = await _db.MusementGuestInfos.Where(s => s.CustomerGuestId == int.Parse(request.In.guestId)).ToListAsync(cancellationToken);

        if (MusementCartInfo != null && MusementCartInfo.Count != 0)
        {
            _db.MusementGuestInfos.RemoveRange(MusementCartInfo);
        }

        var musementGuestInfo = new MusementGuestInfo
        {
            CartUUID = request.In.cartid,
            CustomerGuestId = int.Parse(request.In.guestId)
        };

        await _db.MusementGuestInfos.AddAsync(musementGuestInfo);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new MusementCreateCartOut("Create CartId successful."));
    }
}
