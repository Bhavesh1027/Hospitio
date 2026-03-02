using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;

public class UpdateCustomersDigitalAssistantsOut : BaseResponseOut
{
    public UpdateCustomersDigitalAssistantsOut(string message, UpdatedCustomersDigitalAssistantsOut updatedCustomersPaymentProcessorsOut) : base(message)
    {
        UpdatedCustomersPaymentProcessorsOut = updatedCustomersPaymentProcessorsOut;
    }
    public UpdatedCustomersDigitalAssistantsOut UpdatedCustomersPaymentProcessorsOut { get; set; }
}
public class UpdatedCustomersDigitalAssistantsOut
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Details { get; set; }
    public string? Icon { get; set; }
}