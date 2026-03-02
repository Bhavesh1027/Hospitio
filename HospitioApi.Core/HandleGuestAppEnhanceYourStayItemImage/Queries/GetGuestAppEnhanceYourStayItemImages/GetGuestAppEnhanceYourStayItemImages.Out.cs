using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImages;

public class GetGuestAppEnhanceYourStayItemImagesOut : BaseResponseOut
{
    public GetGuestAppEnhanceYourStayItemImagesOut(string message, List<GuestAppEnhanceYourStayItemImageOut> guestAppEnhanceYourStayItemImageOut) : base(message)
    {
        guestAppEnhanceYourStayItemImageOuts = guestAppEnhanceYourStayItemImageOut;
    }
    public List<GuestAppEnhanceYourStayItemImageOut> guestAppEnhanceYourStayItemImageOuts { get; set; } = new List<GuestAppEnhanceYourStayItemImageOut>();
}
public class GuestAppEnhanceYourStayItemImageOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public string? ItemsImages { get; set; }
    public int? DisaplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public int? FilteredCount { get; set; }
}