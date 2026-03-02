using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;

public class UpdateCustomerRoomServiceOut : BaseResponseOut
{
    public UpdateCustomerRoomServiceOut(string message, UpdatedCustomerRoomServiceOut updatedCustomerRoomServiceOut) : base(message)
    {
        UpdatedCustomerRoomServiceOut = updatedCustomerRoomServiceOut;
    }
    public UpdatedCustomerRoomServiceOut UpdatedCustomerRoomServiceOut { get; set; }
    //public List<UpdatedCustomerRoomServiceOut> UpdatedCustomerRoomServiceOut { get; set; }
}
public class UpdatedCustomerRoomServiceOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }

    public List<UpdatedCustomerRoomServiceItemOut> CustomerRoomServiceItems { get; set; } = new List<UpdatedCustomerRoomServiceItemOut>();

}

public class UpdatedCustomerRoomServiceItemOut
{
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