using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;

public class CreateGuestAppEnhanceYourStayItemImageOut : BaseResponseOut
{
    public CreateGuestAppEnhanceYourStayItemImageOut(string message, List<CreatedEnhanceYourStayItemImageOut> createdEnhanceYourStayItemImageOut) : base(message)
    {
        CreatedEnhanceYourStayItemOut = createdEnhanceYourStayItemImageOut;
    }
    public List<CreatedEnhanceYourStayItemImageOut> CreatedEnhanceYourStayItemOut { get; set; }
}
public class CreatedEnhanceYourStayItemImageOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public string? ItemsImages { get; set; }
    public int? DisaplayOrder { get; set; }

}
