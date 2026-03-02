using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplates;

public class UpdateGuestJourneyMessagesTemplatesOut : BaseResponseOut
{
    public UpdateGuestJourneyMessagesTemplatesOut(string message, UpdatedGuestJourneyMessagesTemplatesOut updatedGuestJourneyMessagesTemplatesOut) : base(message)
    {
        UpdatedGuestJourneyMessagesTemplatesOut = updatedGuestJourneyMessagesTemplatesOut;
    }
    public UpdatedGuestJourneyMessagesTemplatesOut UpdatedGuestJourneyMessagesTemplatesOut { get; set; }
}
public class UpdatedGuestJourneyMessagesTemplatesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WhatsappTemplateName { get; set; } = string.Empty;
    public byte? TempleteType { get; set; }
    public string? TempletMessage { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public bool? IsActive { get; set; }
    public string? Buttons { get; set; }

}
