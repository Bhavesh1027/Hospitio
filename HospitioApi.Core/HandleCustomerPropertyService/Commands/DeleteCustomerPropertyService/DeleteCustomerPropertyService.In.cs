namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;

public class DeleteCustomerPropertyServiceIn
{
    public int Id { get; set; }
}
public class CustomerPropertyServiceJsonOut
{
    public int Id { get; set; }
    public int CustomerPropertyInformationId { get;set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public List<CustomerPropertyInfoServiceImagesOuts> CustomerPropertyInfoServiceImagesOuts { get; set; }  

}
public class CustomerPropertyInfoServiceImagesOuts
{
    public int Id { get; set; }
    public string? ServiceImages { get; set; }
    public bool IsDeleted { get; set; }
}