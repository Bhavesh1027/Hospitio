using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;

public class UpdateCustomerEnhanceYourStayCategoryOut : BaseResponseOut
{
    public UpdateCustomerEnhanceYourStayCategoryOut(string message, UpdatedCustomerEnhanceYourStayCategoryOut updatedCustomerEnhanceYourStayCategoryOut) : base(message)
    {
        UpdatedCustomerEnhanceYourStayCategoryOut = updatedCustomerEnhanceYourStayCategoryOut;
    }
    public UpdatedCustomerEnhanceYourStayCategoryOut UpdatedCustomerEnhanceYourStayCategoryOut { get; set; }
}
public class UpdatedCustomerEnhanceYourStayCategoryOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }

}