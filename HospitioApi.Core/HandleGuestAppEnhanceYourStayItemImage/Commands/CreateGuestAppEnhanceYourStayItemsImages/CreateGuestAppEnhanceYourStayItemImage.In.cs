namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;

public class CreateGuestAppEnhanceYourStayItemImageIn
{
    public int Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public List<GuestAppEnhanceYourStayItemAttachementIn> ItemsImages { get; set; } = new();
}
public class GuestAppEnhanceYourStayItemAttachementIn
{
    public string? ItemsImage { get; set; }
    public int? DisplayOrder { get; set; }
}