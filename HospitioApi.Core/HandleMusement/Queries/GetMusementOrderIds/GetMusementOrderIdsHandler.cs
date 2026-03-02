using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIds;
public record GetMusementOrderIdsRequest(GetMusementOrderIdsIn In) : IRequest<AppHandlerResponse>;
public class GetMusementOrderIdsHandler : IRequestHandler<GetMusementOrderIdsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetMusementOrderIdsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetMusementOrderIdsRequest request, CancellationToken cancellationToken)
    {
        var MusementPaymentInfos = await _db.MusementPaymentInfos.Where(s => s.CustomerId == int.Parse(request.In.CustomerId) && s.CustomerGuestId == int.Parse(request.In.GuestId)).Select(s => s.OrderUUID).ToListAsync(cancellationToken);

        if (MusementPaymentInfos == null || MusementPaymentInfos.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var CustomerNameClass = new GetMusementOrderIdsOutResponse
        {
            OrderId = MusementPaymentInfos
        };

        return _response.Success(new GetMusementOrderIdsOut("Get orderId successful.", CustomerNameClass));
    }
}
