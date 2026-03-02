namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;

public class CreateCustomerEnhanceYourStayIn
{
    public int CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public CategoryName categoryNames { get; set; } = new();
}
public class CategoryName
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public int? CategoryDisplayOrder { get; set; }
    public bool? IsPublish { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CategoryItem> categoryItems { get; set; } = new();

}
public class CategoryItem
{
    public int CategoryItemId { get; set; }
    public bool? IsActive { get; set; }
    public int? ItemDisplayOrder { get; set; }
    public bool? IsPublish { get; set; }
    public bool? IsDeleted { get; set; }

}