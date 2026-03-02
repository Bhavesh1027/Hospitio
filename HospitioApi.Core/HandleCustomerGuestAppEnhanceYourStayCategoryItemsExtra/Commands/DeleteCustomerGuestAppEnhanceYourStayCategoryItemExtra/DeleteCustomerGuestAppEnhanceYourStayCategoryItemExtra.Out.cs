using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra;

public class DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraOut : BaseResponseOut
{
    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraOut(string message, RemoveEnhanceYourStayCategoryItemExtraOut enhanceYourStayItemImageOut) : base(message)
    {
        remove = enhanceYourStayItemImageOut;
    }
    public RemoveEnhanceYourStayCategoryItemExtraOut remove { get; set; }
}
public class RemoveEnhanceYourStayCategoryItemExtraOut
{
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
}