using HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementOrderIdByCartId;

public class GetMusementOrderIdByCartIdOut : BaseResponseOut
{
    public GetMusementOrderIdByCartIdOut(string message, GetMusementOrderIdByCartIdOutResponse getMusementOrderIdByCartIdOutResponseOut) : base(message)
    {
        getMusementOrderIdByCartIdOutResponse = getMusementOrderIdByCartIdOutResponseOut;
    }

    public GetMusementOrderIdByCartIdOutResponse? getMusementOrderIdByCartIdOutResponse { get;set; }
}

public class GetMusementOrderIdByCartIdOutResponse
{
    public string? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
}
