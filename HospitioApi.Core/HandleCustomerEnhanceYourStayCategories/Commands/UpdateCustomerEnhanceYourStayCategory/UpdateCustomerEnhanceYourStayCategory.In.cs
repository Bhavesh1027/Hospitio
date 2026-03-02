namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;

public class UpdateCustomerEnhanceYourStayCategoryIn
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }

}
