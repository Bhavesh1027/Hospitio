using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;

public class DeleteCustomerHouseKeepingOut : BaseResponseOut
{
    public DeleteCustomerHouseKeepingOut(string message, DeletedCustomerHouseKeepingOut deletedCustomerHouseKeepingOut) : base(message)
    {
        DeletedCustomerHouseKeepingOut = deletedCustomerHouseKeepingOut;
    }
    public DeletedCustomerHouseKeepingOut DeletedCustomerHouseKeepingOut { get; set; }
}
public class DeletedCustomerHouseKeepingOut
{
    public int Id { get; set; }
}