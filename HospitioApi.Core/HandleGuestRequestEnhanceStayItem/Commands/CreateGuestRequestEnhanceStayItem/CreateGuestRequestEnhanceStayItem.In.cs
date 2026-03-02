namespace HospitioApi.Core.HandleGuestRequestEnhanceStayItem.Commands.CreateGuestRequestEnhanceStayItem;

public class CreateGuestRequestEnhanceStayItemIn
{
    public int CustomerId { get; set; }
    public int GuestId { get; set; }
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public int? Qty { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentDetails { get; set; }
    public byte? Status { get; set; }
    public List<EnhanceStayItemExtraIn> enhanceStayItemExtraIns { get; set; } = new();
}
public class EnhanceStayItemExtraIn
{
    public int CustomerGuestAppEnhanceYourStayCategoryItemsExtraId { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Year { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public string? PickupLocation { get; set; }
    public int? Qunatity { get; set; }
    public string? Destination { get; set; }
    public string? Comment { get; set; }
    public byte? Status { get; set; }
}