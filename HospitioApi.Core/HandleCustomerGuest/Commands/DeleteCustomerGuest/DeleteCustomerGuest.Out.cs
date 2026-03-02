using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;

public class DeleteCustomerGuestOut : BaseResponseOut
{
    public DeleteCustomerGuestOut(string message, DeletedCustomerGuestOut deletedCustomerGuestOut) : base(message)
    {

        DeletedCustomerGuestOut = deletedCustomerGuestOut;
    }
    public DeletedCustomerGuestOut DeletedCustomerGuestOut { get; set; }
}
public class DeletedCustomerGuestOut
{
    public int Id { get; set; }
}
