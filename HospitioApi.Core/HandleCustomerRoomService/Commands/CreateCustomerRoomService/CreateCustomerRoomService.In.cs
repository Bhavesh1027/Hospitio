

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
public class CreateCustomerRoomServiceIn
{
    public List<CreateCustomerRoomServiceCategoryIn> CustomerRoomServiceCategories { get; set; } = new();
}
public class CreateCustomerRoomServiceCategoryIn
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CreateCustomerRoomServiceItemIn> CustomerRoomServiceItems { get; set; } = new List<CreateCustomerRoomServiceItemIn>();
}

public class CreateCustomerRoomServiceItemIn
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
    public int? DisplayOrder { get; set; }
}