using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;

public class UpdateCustomerOut : BaseResponseOut
{
    public UpdateCustomerOut(string message, UpdatedCustomerOut updatedCustomerOut) : base(message)
    {
        UpdatedCustomerOut = updatedCustomerOut;
    }
    public UpdatedCustomerOut UpdatedCustomerOut { get; set; }
}

public class UpdatedCustomerOut
{
    public int CustomerId { get; set; }
}
