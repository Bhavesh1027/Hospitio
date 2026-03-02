using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneys;

public class DeleteCustomersGuestJourneysOut : BaseResponseOut
{
    public DeleteCustomersGuestJourneysOut(string message, DeletedCustomersGuestJourneysOut deletedCustomersDigitalAssistantsOut) : base(message)
    {
        DeletedCustomersDigitalAssistantsOut = deletedCustomersDigitalAssistantsOut;
    }
    public DeletedCustomersGuestJourneysOut DeletedCustomersDigitalAssistantsOut { get; set; }
}
public class DeletedCustomersGuestJourneysOut
{
    public int Id { get; set; }
}