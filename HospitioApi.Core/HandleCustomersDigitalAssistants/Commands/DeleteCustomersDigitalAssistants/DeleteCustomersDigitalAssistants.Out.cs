using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.DeleteCustomersDigitalAssistants;

public class DeleteCustomersDigitalAssistantsOut : BaseResponseOut
{
    public DeleteCustomersDigitalAssistantsOut(string message, DeletedCustomersDigitalAssistantsOut deletedCustomersPaymentProcessorsOut) : base(message)
    {
        DeletedCustomersPaymentProcessorsOut = deletedCustomersPaymentProcessorsOut;
    }
    public DeletedCustomersDigitalAssistantsOut DeletedCustomersPaymentProcessorsOut { get; set; }
}
public class DeletedCustomersDigitalAssistantsOut
{
    public int Id { get; set; }
}
