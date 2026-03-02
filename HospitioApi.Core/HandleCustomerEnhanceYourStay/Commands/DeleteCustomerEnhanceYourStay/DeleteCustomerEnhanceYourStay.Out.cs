using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStay;

public class DeleteCustomerEnhanceYourStayOut: BaseResponseOut
{
    public DeleteCustomerEnhanceYourStayOut(string message, DeletedCustomerEnhanceYourStayOut deletedCustomerEnhanceYourStayCategoryOut) : base(message)
    {
        DeletedCustomerEnhanceYourStayCategoryOut = deletedCustomerEnhanceYourStayCategoryOut;
    }
    public DeletedCustomerEnhanceYourStayOut DeletedCustomerEnhanceYourStayCategoryOut { get; set; }
}
public class DeletedCustomerEnhanceYourStayOut
{
    public int CategoryId { get; set; }
}