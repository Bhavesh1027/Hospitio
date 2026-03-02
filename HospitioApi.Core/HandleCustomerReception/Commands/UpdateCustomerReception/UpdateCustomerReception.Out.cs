using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;

public class UpdateCustomerReceptionOut : BaseResponseOut
{
    public UpdateCustomerReceptionOut(string message, UpdatedCustomerReceptionOut updatedCustomerReceptionOut) : base(message)
    {
        UpdatedCustomerReceptionOut = updatedCustomerReceptionOut;
    }
    public UpdatedCustomerReceptionOut UpdatedCustomerReceptionOut { get; set; }
    //public List<UpdatedCustomerReceptionOut> UpdatedCustomerReceptionOut { get; set; }
}
public class UpdatedCustomerReceptionOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }

}