using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;

public class GetGuestJourneyMessagesTemplatesOut : BaseResponseOut
{
    public GetGuestJourneyMessagesTemplatesOut(string message, List<GuestJourneyMessagesTemplatesOut> guestJourneyMessagesTemplatesOuts) : base(message)
    {
        GuestJourneyMessagesTemplatesOut = guestJourneyMessagesTemplatesOuts;
    }
    public List<GuestJourneyMessagesTemplatesOut> GuestJourneyMessagesTemplatesOut { get; set; } = new();
}
public class GuestJourneyMessagesTemplatesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte? TempleteType { get; set; }
    public string? TempletMessage { get; set; }
    public string? Buttons { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public bool? IsActive { get; set; }
}
