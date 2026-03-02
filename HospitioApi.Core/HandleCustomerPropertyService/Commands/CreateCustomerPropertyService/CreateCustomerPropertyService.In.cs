using HospitioApi.Data.Models;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;

public class CreateCustomerPropertyServiceIn
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public List<CustomerPropertyServiceImageIn> CustomerPropertyInfoServiceImagesOuts { get; set; } = new List<CustomerPropertyServiceImageIn>();
}
public class CustomerPropertyServiceImageIn
{
    public int Id { get; set; }
    public string? ServiceImages { get; set; }
    public bool IsDeleted { get; set; }

}

