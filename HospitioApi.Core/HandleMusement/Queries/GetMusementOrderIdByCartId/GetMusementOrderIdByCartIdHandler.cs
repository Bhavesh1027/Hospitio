using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIdByCartId;
public record GetMusementOrderIdByCartIdRequest(GetMusementOrderIdByCartIdIn In) : IRequest<AppHandlerResponse>;
public class GetMusementOrderIdByCartIdHandler : IRequestHandler<GetMusementOrderIdByCartIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetMusementOrderIdByCartIdHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetMusementOrderIdByCartIdRequest request, CancellationToken cancellationToken)
    {
        var MusementOrderInfo = await _db.MusementOrderInfos.Where(c => c.CartUUID == request.In.CartId && c.DeletedAt == null).FirstOrDefaultAsync(cancellationToken);

        if (MusementOrderInfo == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var CustomerNameClass = new GetMusementOrderIdByCartIdOutResponse
        {
            OrderId = MusementOrderInfo.OrderUUID,
            OrderDate = MusementOrderInfo.CreatedAt
        };

        return _response.Success(new GetMusementOrderIdByCartIdOut("Get orderId successful.", CustomerNameClass));
    }
}
