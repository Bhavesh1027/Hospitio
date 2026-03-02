using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;

public class CreateGuestJourneyMessagesTemplatesOut : BaseResponseOut
{
    public CreateGuestJourneyMessagesTemplatesOut(string message, CreatedGuestJourneyMessagesTemplatesOut createdGuestJourneyMessagesTemplatesOut) : base(message)
    {
        CreatedGuestJourneyMessagesTemplatesOut = createdGuestJourneyMessagesTemplatesOut;
    }
    public CreatedGuestJourneyMessagesTemplatesOut CreatedGuestJourneyMessagesTemplatesOut { get; set; }
}
public class CreatedGuestJourneyMessagesTemplatesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WhatsappTemplateName { get; set; } = string.Empty;
    public byte? TempleteType { get; set; }
    public string? TempletMessage { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public string? Buttons { get; set; }
}
