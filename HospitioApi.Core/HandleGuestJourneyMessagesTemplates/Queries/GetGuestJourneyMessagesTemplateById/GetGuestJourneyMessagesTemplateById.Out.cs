using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateById;

public class GetGuestJourneyMessagesTemplateByIdOut : BaseResponseOut
{
    public GetGuestJourneyMessagesTemplateByIdOut(string message, GuestJourneyMessagesTemplateByIdOut guestJourneyMessagesTemplateByIdOut) : base(message)
    {
        GuestJourneyMessagesTemplateByIdOut = guestJourneyMessagesTemplateByIdOut;
    }
    public GuestJourneyMessagesTemplateByIdOut GuestJourneyMessagesTemplateByIdOut { get; set; }
}
public class GuestJourneyMessagesTemplateByIdOut
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
