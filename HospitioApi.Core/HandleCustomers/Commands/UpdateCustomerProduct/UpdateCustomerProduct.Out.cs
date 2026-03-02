using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerProduct;

public class UpdateCustomerProductOut: BaseResponseOut
{
    public UpdateCustomerProductOut(string message, UpdatedCustomerProductOut customerProductOut) : base(message)
    {
        UpdatedCustomerProductOut = customerProductOut;
    }
    public UpdatedCustomerProductOut UpdatedCustomerProductOut { get; set; }
}
public class UpdatedCustomerProductOut
{
    public int CustomerId { get; set; }
}
