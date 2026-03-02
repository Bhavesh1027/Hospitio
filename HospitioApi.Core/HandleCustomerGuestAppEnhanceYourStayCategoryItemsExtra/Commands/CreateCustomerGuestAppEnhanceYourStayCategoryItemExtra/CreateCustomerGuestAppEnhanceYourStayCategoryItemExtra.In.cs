namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;

public class CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn
{
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public List<CreateCustomerGuestAppEnhanceYourStayCategoryItem> createCustomerGuestAppEnhanceYourStayCategoryItems { get; set; }
}
public class CreateCustomerGuestAppEnhanceYourStayCategoryItem
{
    public int? Id { get; set; }
    public byte? QueType { get; set; }
    public string? Questions { get; set; }
    public string? OptionValues { get; set; }
}
