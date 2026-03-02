using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;

public class UpdateCustomerHouseKeepingOut : BaseResponseOut
{
    public UpdateCustomerHouseKeepingOut(string message, UpdatedCustomerHouseKeepingOut updatedCustomerHouseKeepingOut) : base(message)
    {
        UpdatedCustomerHouseKeepingOut = updatedCustomerHouseKeepingOut;
    }
    public UpdatedCustomerHouseKeepingOut UpdatedCustomerHouseKeepingOut { get; set; }
    //public List<UpdatedCustomerHouseKeepingOut> UpdatedCustomerHouseKeepingOut { get; set; }
}
public class UpdatedCustomerHouseKeepingOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
}