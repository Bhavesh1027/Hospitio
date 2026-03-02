namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;

public class DeleteCustomerPropertyEmergencyNumberIn
{
    public int Id { get; set; }
}
public class CustomerPropertyEmergencyNumberJsonOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }
}
