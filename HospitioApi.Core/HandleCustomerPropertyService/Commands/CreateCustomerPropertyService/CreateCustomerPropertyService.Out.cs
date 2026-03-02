using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;

public class CreateCustomerPropertyServiceOut : BaseResponseOut
{
    public CreateCustomerPropertyServiceOut(string message, CreatedCustomerPropertyServiceOut createdCustomerPropertyServiceOut) : base(message)
    {
        CustomerPropertyInfoServiceImagesOuts = createdCustomerPropertyServiceOut;
    }
    public CreatedCustomerPropertyServiceOut CustomerPropertyInfoServiceImagesOuts { get; set; }
}
public class CreatedCustomerPropertyServiceOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
}

