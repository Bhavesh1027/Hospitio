using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;

public class CreateCustomerHouseKeepingOut : BaseResponseOut
{
    public CreateCustomerHouseKeepingOut(string message, List<CreatedCustomerHouseKeepingOut> createdCustomerHouseKeepingOut) : base(message)
    {
        CreatedCustomerHouseKeepingOut = createdCustomerHouseKeepingOut;
    }
    public List<CreatedCustomerHouseKeepingOut> CreatedCustomerHouseKeepingOut { get; set; }
}
public class CreatedCustomerHouseKeepingOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
}
