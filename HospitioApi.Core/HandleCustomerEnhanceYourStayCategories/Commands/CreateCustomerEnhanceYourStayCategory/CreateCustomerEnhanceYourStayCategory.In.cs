namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.CreateCustomerEnhanceYourStayCategory;

public class CreateCustomerEnhanceYourStayCategoryIn
{
    public int CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
}