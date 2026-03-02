using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId;

public class GetCustomerGuestAppEnhannceYourStayItemExtraOut : BaseResponseOut
{
    public GetCustomerGuestAppEnhannceYourStayItemExtraOut(string message, List<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut> guestAppEnhanceYourStayCategoryItemsExtraOuts) : base(message)
    {
        customers = guestAppEnhanceYourStayCategoryItemsExtraOuts;
    }
    public List<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut> customers { get; set; } = new List<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut>();
}
public class CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut
{
    public int? Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public byte? QueType { get; set; }
    public string? Questions { get; set; }
    public string? OptionValues { get; set; }
    public bool? IsActive { get; set; }
}