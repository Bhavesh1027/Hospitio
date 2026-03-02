using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;

public class CreateCustomersDigitalAssistantsOut : BaseResponseOut
{
    public CreateCustomersDigitalAssistantsOut(string message, CreatedCustomersDigitalAssistantsOut createdCustomersDigitalAssistantsOut) : base(message)
    {
        CreatedCustomersDigitalAssistantsOut = createdCustomersDigitalAssistantsOut;
    }
    public CreatedCustomersDigitalAssistantsOut CreatedCustomersDigitalAssistantsOut { get; set; }
}
public class CreatedCustomersDigitalAssistantsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public string? Icon { get; set; }
}