using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;

public class UpdateCustomerPropertyEmergencyNumberOut : BaseResponseOut
{
    public UpdateCustomerPropertyEmergencyNumberOut(string message, UpdatedCustomerPropertyEmergencyNumberOut updatedCustomerPropertyEmergencyNumberOut) : base(message)
    {
        UpdatedCustomerPropertyEmergencyNumberOut = updatedCustomerPropertyEmergencyNumberOut;
    }
    public UpdatedCustomerPropertyEmergencyNumberOut UpdatedCustomerPropertyEmergencyNumberOut { get; set; }
}

public class UpdatedCustomerPropertyEmergencyNumberOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DisplayOrder { get; set; }

}
