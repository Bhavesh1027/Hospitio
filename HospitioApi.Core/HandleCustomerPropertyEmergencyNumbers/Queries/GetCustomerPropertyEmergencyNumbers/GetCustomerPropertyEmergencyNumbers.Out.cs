using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumbers;

public class GetCustomerPropertyEmergencyNumbersOut : BaseResponseOut
{
    public GetCustomerPropertyEmergencyNumbersOut(string message , List<CustomerPropertyEmergencyNumbersOut> customerPropertyEmergencyNumbersOuts) : base(message)
    {
        CustomerPropertyEmergencyNumbers = customerPropertyEmergencyNumbersOuts;
    }
    public List<CustomerPropertyEmergencyNumbersOut> CustomerPropertyEmergencyNumbers { get;set; } = new List<CustomerPropertyEmergencyNumbersOut>();
 }

public class CustomerPropertyEmergencyNumbersOut
{
    public int Id { get; set; }
    public int? CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DisplayOrder { get; set; }

}
