using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;

public class CustomerGuestAppEnhanceYourStayCategoryItemExtraOut : BaseResponseOut
{
    public CustomerGuestAppEnhanceYourStayCategoryItemExtraOut(string message, CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut created) : base(message)
    {
        createdCustomers = created;
    }
    public CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut createdCustomers { get; set; }
}
public class CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut
{
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
}