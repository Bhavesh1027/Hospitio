namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;

public class CreateCustomerHouseKeepingIn
{
    public List<CreateCustomerHouseKeepingCategoryIn> CustomerHouseKeepingCategories { get; set; } = new List<CreateCustomerHouseKeepingCategoryIn>();
}

public class CreateCustomerHouseKeepingCategoryIn
{
    public int CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CreateCustomerHouseKeepingItemIn> CustomerHouseKeepingItems { get; set; } = new List<CreateCustomerHouseKeepingItemIn>();
}

public class CreateCustomerHouseKeepingItemIn
{
    public int? CustomerId { get; set; }
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
}