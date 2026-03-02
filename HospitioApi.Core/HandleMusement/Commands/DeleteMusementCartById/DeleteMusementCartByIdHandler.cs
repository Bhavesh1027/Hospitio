using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Commands.DeleteMusementCartById;
public record DeleteMusementCartByIdRequest(DeleteMusementCartById In) : IRequest<AppHandlerResponse>;
public class DeleteMusementCartByIdHandler : IRequestHandler<DeleteMusementCartByIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteMusementCartByIdHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteMusementCartByIdRequest request, CancellationToken cancellationToken)
    {

        var musementCartInfo = await _db.MusementGuestInfos.Where(s => s.CartUUID == request.In.CartId && s.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

        if (musementCartInfo == null)
        {
            return _response.Error("Cart Does not exits", AppStatusCodeError.Gone410);
        }

        _db.MusementGuestInfos.Remove(musementCartInfo);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteMusementCartByIdOut("Cart Delete successful."));
    }
}
