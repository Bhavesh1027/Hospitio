namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;


public class UpdateCustomerPropertyEmergencyNumberIn
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }
    public int? DisplayOrder { get; set; }
}
