using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplates;

public class DeleteGuestJourneyMessagesTemplatesOut : BaseResponseOut
{
    public DeleteGuestJourneyMessagesTemplatesOut(string message, DeletedGuestJourneyMessagesTemplatesOut deletedGuestJourneyMessagesTemplatesOut) : base(message)
    {
        DeletedGuestJourneyMessagesTemplatesOut = deletedGuestJourneyMessagesTemplatesOut;
    }
    public DeletedGuestJourneyMessagesTemplatesOut DeletedGuestJourneyMessagesTemplatesOut { get; set; }
}
public class DeletedGuestJourneyMessagesTemplatesOut
{
    public int Id { get; set; }
}
