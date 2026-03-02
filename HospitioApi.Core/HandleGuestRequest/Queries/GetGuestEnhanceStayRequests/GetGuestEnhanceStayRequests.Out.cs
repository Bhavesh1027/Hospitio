using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestEnhanceStayRequests;

public class GetGuestEnhanceStayRequestsOut : BaseResponseOut
{
    public GetGuestEnhanceStayRequestsOut(string message, List<GuestRequestsOut> guestRequestsOut) : base(message)
    {
        GuestRequestsOut = guestRequestsOut;
    }
    public List<GuestRequestsOut> GuestRequestsOut { get; set; }
}
public class GuestRequestsOut
{
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public byte? Status { get; set; }
    public byte? RequestType { get; set; }
}