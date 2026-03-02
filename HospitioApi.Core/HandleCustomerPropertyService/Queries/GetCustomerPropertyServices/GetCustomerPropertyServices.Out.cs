using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServices;

public class GetCustomerPropertyServicesOut : BaseResponseOut
{
    public GetCustomerPropertyServicesOut(string message, List<CustomerPropertyServicesOut> customerPropertyServicesOut) : base(message)
    {
        CustomerPropertyServicesOut = customerPropertyServicesOut;
    }
    public List<CustomerPropertyServicesOut> CustomerPropertyServicesOut { get; set; }
}
public class CustomerPropertyServicesOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public List<CustomerPropertyServiceImageOut> CustomerPropertyInfoServiceImagesOuts { get; set; } = new List<CustomerPropertyServiceImageOut>();
}
public class CustomerPropertyServiceImageOut
{
    public int Id { get; set; }
    public int? CustomerPropertyServiceId { get; set; }
    public string? ServiceImages { get; set; }
    public bool IsDeleted { get; set; }
}