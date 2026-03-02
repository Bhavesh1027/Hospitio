using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;

public class GetCustomerEnhanceYourStayOut: BaseResponseOut
{
    public GetCustomerEnhanceYourStayOut(string message, List<CustomerEnhanceYourStayOut> customerEnhanceYourStayCategoriesOut) : base(message)
    {
        CustomersEnhanceYourStayCategoriesOut = customerEnhanceYourStayCategoriesOut;
    }
    public List<CustomerEnhanceYourStayOut> CustomersEnhanceYourStayCategoriesOut { get; set; } = new List<CustomerEnhanceYourStayOut>();
}
public class CustomerEnhanceYourStayOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerId { get; set; }
    public string? CategoryName { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerGuestAppEnhanceYourStayItems> customerGuestAppEnhanceYourStayItems { get; set; } = new();
}
public class CustomerGuestAppEnhanceYourStayItems
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set;}
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderCategoryId { get; set; }
    public byte? Badge { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public byte? ButtonType { get;set; }
    public string? ButtonText { get; set; }
    public byte? ChargeType { get;set; }
    public decimal? Discount { get;set; }
    public decimal? Price { get;set; }
    public string? Currency { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerGuestAppEnhanceYourStayItemsImages> customerGuestAppEnhanceYourStayItemsImages { get; set; } = new();
   
}
public class CustomerGuestAppEnhanceYourStayItemsImages
{
    public int? Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public string? ItemsImages { get;set; }
    public int? DisaplayOrder { get; set; }
    public bool? IsActive { get; set; }
}