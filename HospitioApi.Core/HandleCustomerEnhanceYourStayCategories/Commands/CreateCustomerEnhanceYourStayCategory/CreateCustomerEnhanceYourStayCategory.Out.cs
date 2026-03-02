using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.CreateCustomerEnhanceYourStayCategory;

public class CreateCustomerEnhanceYourStayCategoryOut : BaseResponseOut
{
    public CreateCustomerEnhanceYourStayCategoryOut(string message, CreatedCustomerEnhanceYourStayCategoryOut createdCustomerEnhanceYourStayCategoryOut) : base(message)
    {
        CreatedCustomerEnhanceYourStayCategoryOut = createdCustomerEnhanceYourStayCategoryOut;
    }
    public CreatedCustomerEnhanceYourStayCategoryOut CreatedCustomerEnhanceYourStayCategoryOut { get; set; }
}
public class CreatedCustomerEnhanceYourStayCategoryOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
}