using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;

public class DeleteCustomerRoomServiceOut : BaseResponseOut
{
    public DeleteCustomerRoomServiceOut(string message, DeletedCustomerRoomServiceOut deletedCustomerRoomServiceOut) : base(message)
    {
        DeletedCustomerRoomServiceOut = deletedCustomerRoomServiceOut;
    }
    public DeletedCustomerRoomServiceOut DeletedCustomerRoomServiceOut { get; set; }
}
public class DeletedCustomerRoomServiceOut
{
    public int Id { get; set; }
}