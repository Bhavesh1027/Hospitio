using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;

public class GetCustomerRoomServiceWithRelationOut : BaseResponseOut
{
    public GetCustomerRoomServiceWithRelationOut(string message, List<CustomerRoomServiceWithRelationOut> customerRoomServiceWithRelationOut) : base(message)
    {
        CustomerRoomServiceWithRelationOut = customerRoomServiceWithRelationOut;
    }
    public List<CustomerRoomServiceWithRelationOut> CustomerRoomServiceWithRelationOut { get; set; } = new List<CustomerRoomServiceWithRelationOut>();
}
public class CustomerRoomServiceWithRelationOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerRoomServiceItemsRelationOut> CustomerRoomServiceItems { get; set; } = new List<CustomerRoomServiceItemsRelationOut>();
}

public class CustomerRoomServiceItemsRelationOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
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
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
}