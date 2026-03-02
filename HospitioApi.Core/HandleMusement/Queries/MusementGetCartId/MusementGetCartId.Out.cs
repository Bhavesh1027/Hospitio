using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Queries.MusementGetCartId;

public class MusementGetCartIdOut : BaseResponseOut
{
    public MusementGetCartIdOut(string message, MusementGetCartIdResponse musementGetCartIdResponseOut) : base(message)
    {
        musementGetCartIdResponse = musementGetCartIdResponseOut;
    }
    public MusementGetCartIdResponse? musementGetCartIdResponse { get;set; }

}
public class MusementGetCartIdResponse
{
    public string? cartId { get; set; }

}