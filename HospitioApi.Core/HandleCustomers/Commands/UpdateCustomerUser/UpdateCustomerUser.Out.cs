using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerUser;

public class UpdateCustomerUserOut : BaseResponseOut
{
    public UpdateCustomerUserOut(string message, UpdatedCustomerOut createdCustomerOut) : base(message)
    {
        CreatedCustomerOut = createdCustomerOut;
    }
    public UpdatedCustomerOut CreatedCustomerOut { get; set; }
}
public class UpdatedCustomerOut
{
    public int CustomerId { get; set; }

}