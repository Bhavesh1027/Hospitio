using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;

public class CreateCustomerOut : BaseResponseOut
{
    public CreateCustomerOut(string message, CreatedCustomerOut createdCustomerOut) : base(message)
    {
        CreatedCustomerOut = createdCustomerOut;
    }
    public CreatedCustomerOut CreatedCustomerOut { get; set; }

}
public class CreatedCustomerOut
{
    public int CustomerId { get; set; }
    public string? UserUniqueId { get; set; }
    public string BusinessName { get; set; } = string.Empty;

}
