namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.UpdateCustomerPropertyService;

public class UpdateCustomerPropertyServiceIn
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public List<UPCustomerPropertyServiceImageIn> UPCustomerPropertyServiceImageIns { get; set; } = new List<UPCustomerPropertyServiceImageIn>();

}
public class UPCustomerPropertyServiceImageIn
{
    public int Id { get; set; }
    public string? ServiceImage { get; set; }
}