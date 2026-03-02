using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;

public class GetGuestRequestsOut : BaseResponseOut
{
    public GetGuestRequestsOut(string message, List<GuestRequestsOut> guestRequestsOut) : base(message)
    {
        GuestRequestsOut = guestRequestsOut;
    }
    public List<GuestRequestsOut> GuestRequestsOut { get; set; }
}
public class GuestRequestsOut
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public string? GuestName { get; set; }
    public string? GuestStatus { get; set; }
    //public string? Room { get; set; }
    public string? Department { get; set; }
    public string? TaskItem { get; set; }
    public decimal? Charge { get; set; }
    public DateTime? TimeStamp { get; set; }
    public int? TaskStatus { get; set; }
    public int? Rating { get; set; }
    public int? TotalCount { get; set; }
    public string? GuestRequest { get; set; }
}
