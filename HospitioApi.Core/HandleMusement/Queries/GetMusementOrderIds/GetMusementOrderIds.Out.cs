using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIds;

public class GetMusementOrderIdsOut : BaseResponseOut
{
    public GetMusementOrderIdsOut(string message, GetMusementOrderIdsOutResponse getMusementOrderIdsOutResponseOut) : base(message)
    {
        getMusementOrderIdsOutResponse = getMusementOrderIdsOutResponseOut;
    }
    public GetMusementOrderIdsOutResponse getMusementOrderIdsOutResponse { get; set; }
}
public class GetMusementOrderIdsOutResponse
{
    public List<string>? OrderId { get; set; }
}
