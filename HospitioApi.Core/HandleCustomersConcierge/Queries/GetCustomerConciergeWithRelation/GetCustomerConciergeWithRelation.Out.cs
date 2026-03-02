using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;

public class GetCustomerConciergeWithRelationOut : BaseResponseOut
{
    public GetCustomerConciergeWithRelationOut(string message, List<CustomerConciergeWithRelationOut> customerConciergeWithRelationOut) : base(message)
    {
        CustomerConciergeWithRelationOut = customerConciergeWithRelationOut;
    }
    public List<CustomerConciergeWithRelationOut> CustomerConciergeWithRelationOut { get; set; } = new();
}
public class CustomerConciergeWithRelationOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerConciergeItemsRelationOut> CustomerConciergeItems { get; set; } = new();
}

public class CustomerConciergeItemsRelationOut
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
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
}