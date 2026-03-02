using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;
public record MusementGetCartIdRequest(MusementGetCartIdIn In) : IRequest<AppHandlerResponse>;
public class MusementGetCartIdHandler : IRequestHandler<MusementGetCartIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public MusementGetCartIdHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(MusementGetCartIdRequest request, CancellationToken cancellationToken)
    {
        var MusementGuestInfo = await _db.MusementGuestInfos.Where(c => c.CustomerGuestId == request.In.GuestId && c.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

        if (MusementGuestInfo == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var CustomerNameClass = new MusementGetCartIdResponse
        {
            cartId = MusementGuestInfo.CartUUID
        };

        return _response.Success(new MusementGetCartIdOut("Get CartId successful.", CustomerNameClass));
    }
}
