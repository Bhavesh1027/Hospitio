using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;

public class CreateCustomerEnhanceYourStayOut: BaseResponseOut
{
    public CreateCustomerEnhanceYourStayOut(string message, CreatedCustomerEnhanceYourStayOut createdCustomerEnhanceYourStayOut) : base(message)
    {
        CreatedCustomerEnhanceYourStayCategoryOut = createdCustomerEnhanceYourStayOut;
    }
    public CreatedCustomerEnhanceYourStayOut CreatedCustomerEnhanceYourStayCategoryOut { get; set; }
}
public class CreatedCustomerEnhanceYourStayOut
{
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
}