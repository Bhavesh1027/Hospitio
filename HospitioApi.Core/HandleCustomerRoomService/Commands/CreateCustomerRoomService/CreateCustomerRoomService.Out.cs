using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;

public class CreateCustomerRoomServiceOut : BaseResponseOut
{
    public CreateCustomerRoomServiceOut(string message, List<CreatedCustomerRoomServiceOut> createdCustomerRoomServiceOut) : base(message)
    {
        CreatedCustomerRoomServiceOut = createdCustomerRoomServiceOut;
    }
    public List<CreatedCustomerRoomServiceOut> CreatedCustomerRoomServiceOut { get; set; }
}
public class CreatedCustomerRoomServiceOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CreatedCustomerRoomServiceItemOut> CustomerRoomServiceItems { get; set; } = new List<CreatedCustomerRoomServiceItemOut>();
}

public class CreatedCustomerRoomServiceItemOut
{
    public int? CustomerId { get; set; }
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
}