using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;

public class CreateCustomerConciergeOut : BaseResponseOut
{
    public CreateCustomerConciergeOut(string message, List<CreatedCustomerConciergeOut> createdCustomerConciergeOut) : base(message)
    {
        CreatedCustomerConciergeOut = createdCustomerConciergeOut;
    }
    public List<CreatedCustomerConciergeOut> CreatedCustomerConciergeOut { get; set; }
}
public class CreatedCustomerConciergeOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }

}