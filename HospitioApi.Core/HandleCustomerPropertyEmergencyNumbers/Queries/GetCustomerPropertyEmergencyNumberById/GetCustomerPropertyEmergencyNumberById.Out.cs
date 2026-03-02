using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;

public class GetCustomerPropertyEmergencyNumberByIdOut : BaseResponseOut
{
    public GetCustomerPropertyEmergencyNumberByIdOut(string message, CustomerPropertyEmergencyNumberByIdOut customerPropertyEmergencyNumberByIdOut) : base(message)
    {
        CustomerPropertyEmergencyNumberByIdOut = customerPropertyEmergencyNumberByIdOut;
    }
    public CustomerPropertyEmergencyNumberByIdOut CustomerPropertyEmergencyNumberByIdOut { get; set; }
}

public class CustomerPropertyEmergencyNumberByIdOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
}
