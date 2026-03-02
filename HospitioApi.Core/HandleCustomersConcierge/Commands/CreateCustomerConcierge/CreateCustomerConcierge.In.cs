namespace HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;

public class CreateCustomerConciergeIn
{
    public List<CreateCustomerConciergeCategoryIn> CustomerConciergeCategories { get; set; } = new List<CreateCustomerConciergeCategoryIn>();
}

public class CreateCustomerConciergeCategoryIn
{
    public int CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CreateCustomerConciergeItemIn> CustomerConciergeItems { get; set; } = new List<CreateCustomerConciergeItemIn>();
}

public class CreateCustomerConciergeItemIn
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