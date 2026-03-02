using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;

public class CreateCustomerReceptionOut : BaseResponseOut
{
    public CreateCustomerReceptionOut(string message, List<CreatedCustomerReceptionOut> createdCustomerReceptionOut) : base(message)
    {
        CreatedCustomerReceptionOut = createdCustomerReceptionOut;
    }
    public List<CreatedCustomerReceptionOut> CreatedCustomerReceptionOut { get; set; }
}
public class CreatedCustomerReceptionOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
}

