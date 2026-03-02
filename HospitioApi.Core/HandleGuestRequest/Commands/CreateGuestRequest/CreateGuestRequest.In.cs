using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;

public class CreateGuestRequestIn
{
    public List<GuestRequestIn> GuestRequests { get; set; } = new();
}
public class GuestRequestIn
{
    public int CustomerId { get; set; }
    public GuestRequestTypeEnum RequestType { get; set; }
    public int? CustomerGuestAppConciergeItemId { get; set; }
    //public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public int? CustomerGuestAppHousekeepingItemId { get; set; }
    public int? CustomerGuestAppRoomServiceItemId { get; set; }
    public int? CustomerGuestAppReceptionItemId { get; set; }
    public int GuestId { get; set; }
    public int? MonthValue { get; set; }
    public int? DayValue { get; set; }
    public int? YearValue { get; set; }
    public int? HourValue { get; set; }
    public int? MinuteValue { get; set; }
    public string? PickupLocation { get; set; }
    public string? Destination { get; set; }
    public string? Comment { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentDetails { get; set; }
    public GuestRequestStatusEnum? Status { get; set; }
    public bool IsActive { get; set; }
    public int? QuantityBar { get; set; }
}
