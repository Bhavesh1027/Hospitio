using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;

public class GetCustomersDigitalAssistantsOut : BaseResponseOut
{
    public GetCustomersDigitalAssistantsOut(string message, List<CustomersDigitalAssistantsOut> customersDigitalAssistantsOut) : base(message)
    {
        CustomersDigitalAssistantsOut = customersDigitalAssistantsOut;
    }
    public List<CustomersDigitalAssistantsOut> CustomersDigitalAssistantsOut { get; set; } = new List<CustomersDigitalAssistantsOut>();
}
public class CustomersDigitalAssistantsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public string? Icon { get; set; }
    public bool? IsActive { get; set; }
    //public int? FilteredCount { get; set; }
}
