namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;

public class UpdateCustomerHouseKeepingIn
{
    public UpdateCustomerHouseKeepingCategoryIn CustomerHouseKeepingCategories { get; set; } = new ();
}
public class UpdateCustomerHouseKeepingCategoryIn
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<UpdateCustomerHouseKeepingItemIn> CustomerHouseKeepingItems { get; set; } = new List<UpdateCustomerHouseKeepingItemIn>();

}

public class UpdateCustomerHouseKeepingItemIn
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? Name { get; set; }
    public bool? ItemsMonth { get; set; }
    public bool? ItemsDay { get; set; }
    public bool? ItemsMinute { get; set; }
    public bool? ItemsHour { get; set; }
    public bool? QuantityBar { get; set; }
    public bool? ItemLocation { get; set; }
    public bool? Comment { get; set; }
    public bool? IsPriceEnable { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsActive { get; set; }
}
