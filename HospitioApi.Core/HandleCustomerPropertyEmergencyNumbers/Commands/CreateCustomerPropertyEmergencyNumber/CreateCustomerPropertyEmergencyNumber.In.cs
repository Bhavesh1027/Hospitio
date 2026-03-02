namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;

public class CreateCustomerPropertyEmergencyNumberIn
{
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
}
