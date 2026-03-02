using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequestById;

public class GetGuestRequestByIdOut : BaseResponseOut
{
    public GetGuestRequestByIdOut(string message, GuestRequestByIdOut guestRequestByIdOut) : base(message)
    {
        GuestRequestByIdOut = guestRequestByIdOut;
    }
    public GuestRequestByIdOut GuestRequestByIdOut { get; set; }
}
public class GuestRequestByIdOut
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? TaskStatus { get; set; }
    public string? Room { get; set; }
    public int? RequestType { get; set; }
    public string? EnhanceYourStayItem { get; set; }
    public decimal? EnhanceYourStayItemPrice { get; set; }
    public string? HouseKeepingItem { get; set; }
    public decimal? HouseKeepingItemPrice { get; set; }
    public string? ConciergeItem { get; set; }
    public decimal? ConciergeItemPrice { get; set; }
    public string? ReceptionItem { get; set; }
    public decimal? ReceptionItemPrice { get; set; }
    public string? RoomServiceItem { get; set; }
    public decimal? RoomServiceItemPrice { get; set; }
    public int Rating { get; set; }
    public int? MonthValue { get; set; }
    public int? DayValue { get; set; }
    public int? YearValue { get; set; }
    public int? HourValue { get; set; }
    public int? MinuteValue { get; set; }
    public DateTime? TimeStamp { get; set; }
    public DateTime? UpdateAt { get; set; }
}
