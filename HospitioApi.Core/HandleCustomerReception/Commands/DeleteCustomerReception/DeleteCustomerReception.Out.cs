using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;

public class DeleteCustomerReceptionOut : BaseResponseOut
{
    public DeleteCustomerReceptionOut(string message, DeletedCustomerReceptionOut deletedCustomerReceptionOut) : base(message)
    {
        DeletedCustomerReceptionOut = deletedCustomerReceptionOut;
    }
    public DeletedCustomerReceptionOut DeletedCustomerReceptionOut { get; set; }
}
public class DeletedCustomerReceptionOut
{
    public int Id { get; set; }
}