using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;

public class CreateERPCustomerOut : BaseResponseOut
{
    public CreateERPCustomerOut(string message, CreatedERPCustomerOut createdCustomerOut) : base(message)
    {
        CreatedCustomerOut = createdCustomerOut;
    }
    public CreatedERPCustomerOut CreatedCustomerOut { get; set; }
}
public class CreatedERPCustomerOut
{
    public string? PylonUniqueCustomerId { get; set; }

}