using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;

public class UpdateCustomerConciergeOut : BaseResponseOut
{
    public UpdateCustomerConciergeOut(string message, UpdatedCustomerConciergeOut updatedCustomerConciergeOut) : base(message)
    {
        UpdatedCustomerConciergeOut = updatedCustomerConciergeOut;
    }
    public UpdatedCustomerConciergeOut UpdatedCustomerConciergeOut { get; set; }
    //public List<UpdatedCustomerConciergeOut> UpdatedCustomerConciergeOut { get; set; }
}
public class UpdatedCustomerConciergeOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
}

