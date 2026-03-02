using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;

public class GetGuestJourneyMessagesTemplatesForCustomerOut : BaseResponseOut
{
    public GetGuestJourneyMessagesTemplatesForCustomerOut(string message, List<GuestJourneyMessagesTemplatesForCustomerOut> guestJourneyMessagesTemplatesOuts) : base(message)
    {
        GuestJourneyMessagesTemplatesOut = guestJourneyMessagesTemplatesOuts;
    }
    public List<GuestJourneyMessagesTemplatesForCustomerOut> GuestJourneyMessagesTemplatesOut { get; set; } = new();
}
public class GuestJourneyMessagesTemplatesForCustomerOut
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
