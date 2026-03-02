using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategory;

public class DeleteCustomerEnhanceYourStayCategoryOut : BaseResponseOut
{
    public DeleteCustomerEnhanceYourStayCategoryOut(string message, DeletedCustomerEnhanceYourStayCategoryOut deletedCustomerEnhanceYourStayCategoryOut) : base(message)
    {
        DeletedCustomerEnhanceYourStayCategoryOut = deletedCustomerEnhanceYourStayCategoryOut;
    }
    public DeletedCustomerEnhanceYourStayCategoryOut DeletedCustomerEnhanceYourStayCategoryOut { get; set; }
}
public class DeletedCustomerEnhanceYourStayCategoryOut
{
    public int Id { get; set; }
}