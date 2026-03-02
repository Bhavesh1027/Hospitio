using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.CreateCustomerPropertyEmergencyNumber;

public class CreateCustomerPropertyEmergencyNumberOut : BaseResponseOut
{
    public CreateCustomerPropertyEmergencyNumberOut(string message, CreatedCustomerPropertyEmergencyNumberOut createdCustomerPropertyEmergencyNumberOut) : base(message)
    {
        CreatedCustomerPropertyEmergencyNumberOut = createdCustomerPropertyEmergencyNumberOut;
    }
    public CreatedCustomerPropertyEmergencyNumberOut CreatedCustomerPropertyEmergencyNumberOut { get; set; }
}

public class CreatedCustomerPropertyEmergencyNumberOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
}
