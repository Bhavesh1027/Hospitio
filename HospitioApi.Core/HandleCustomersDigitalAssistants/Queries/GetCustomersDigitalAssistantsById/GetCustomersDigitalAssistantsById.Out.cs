using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;

public class GetCustomersDigitalAssistantsByIdOut : BaseResponseOut
{
    public GetCustomersDigitalAssistantsByIdOut(string message, CustomersDigitalAssistantsByIdOut customersDigitalAssistantsByIdOut) : base(message)
    {
        CustomersDigitalAssistantsByIdOut = customersDigitalAssistantsByIdOut;
    }
    public CustomersDigitalAssistantsByIdOut CustomersDigitalAssistantsByIdOut { get; set; }
}
public class CustomersDigitalAssistantsByIdOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public string? Icon { get; set; }
}
