using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;

public class GetCustomerHouseKeepingWithRelationOut : BaseResponseOut
{
    public GetCustomerHouseKeepingWithRelationOut(string message, List<CustomerHouseKeepingWithRelationOut> customerHouseKeepingWithRelationOut) : base(message)
    {
        CustomerHouseKeepingWithRelationOut = customerHouseKeepingWithRelationOut;
    }
    public List<CustomerHouseKeepingWithRelationOut> CustomerHouseKeepingWithRelationOut { get; set; } = new List<CustomerHouseKeepingWithRelationOut>();
}
public class CustomerHouseKeepingWithRelationOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerHouseKeepingItemsRelationOut> CustomerHouseKeepingItems { get; set; } = new List<CustomerHouseKeepingItemsRelationOut>();
}

public class CustomerHouseKeepingItemsRelationOut
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