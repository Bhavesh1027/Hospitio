using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryById;

public class GetCustomerEnhanceYourStayCategoryByIdOut : BaseResponseOut
{
    public GetCustomerEnhanceYourStayCategoryByIdOut(string message, CustomerEnhanceYourStayCategoryByIdOut customerEnhanceYourStayCategoryByIdOut) : base(message)
    {
        CustomerEnhanceYourStayCategoryByIdOut = customerEnhanceYourStayCategoryByIdOut;
    }
    public CustomerEnhanceYourStayCategoryByIdOut CustomerEnhanceYourStayCategoryByIdOut { get; set; }
}
public class CustomerEnhanceYourStayCategoryByIdOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }

}