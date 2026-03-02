using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;

public class DeleteCustomerEnhanceYourStayItemOut : BaseResponseOut
{
    public DeleteCustomerEnhanceYourStayItemOut(string message, DeletedCustomerEnhanceYourStayItemOut deletedCustomerEnhanceYourStayItemOut) : base(message)
    {
        DeletedCustomerEnhanceYourStayItemOut = deletedCustomerEnhanceYourStayItemOut;
    }
    public DeletedCustomerEnhanceYourStayItemOut DeletedCustomerEnhanceYourStayItemOut { get; set; }
}
public class DeletedCustomerEnhanceYourStayItemOut
{
    public int Id { get; set; }
}