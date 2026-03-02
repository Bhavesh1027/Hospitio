using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategories;

public class GetCustomerEnhanceYourStayCategoriesOut : BaseResponseOut
{
    public GetCustomerEnhanceYourStayCategoriesOut(string message, List<CustomerEnhanceYourStayCategoriesOut> customerEnhanceYourStayCategoriesOut) : base(message)
    {
        CustomersEnhanceYourStayCategoriesOut = customerEnhanceYourStayCategoriesOut;
    }
    public List<CustomerEnhanceYourStayCategoriesOut> CustomersEnhanceYourStayCategoriesOut { get; set; } = new List<CustomerEnhanceYourStayCategoriesOut>();
}
public class CustomerEnhanceYourStayCategoriesOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? FilteredCount { get; set; }

}