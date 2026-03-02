using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;

public class GetCustomerReceptionWithRelationOut : BaseResponseOut
{
    public GetCustomerReceptionWithRelationOut(string message, List<CustomerReceptionWithRelationOut> customerReceptionWithRelationOut) : base(message)
    {
        CustomerReceptionWithRelationOut = customerReceptionWithRelationOut;
    }
    public List<CustomerReceptionWithRelationOut> CustomerReceptionWithRelationOut { get; set; } = new List<CustomerReceptionWithRelationOut>();
}
public class CustomerReceptionWithRelationOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerReceptionItemsRelationOut> CustomerReceptionItems { get; set; } = new List<CustomerReceptionItemsRelationOut>();
}

public class CustomerReceptionItemsRelationOut
{
    public int Id { get; set; }
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
    public bool? IsDeleted { get; set; }
    public bool? IsActive { get; set; }
}