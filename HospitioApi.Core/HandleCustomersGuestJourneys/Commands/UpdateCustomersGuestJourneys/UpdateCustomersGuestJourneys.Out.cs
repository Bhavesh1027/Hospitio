using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;

public class UpdateCustomersGuestJourneysOut : BaseResponseOut
{
    public UpdateCustomersGuestJourneysOut(string message, UpdatedCustomersGuestJourneysOut updatedCustomersGuestJourneysOut) : base(message)
    {
        UpdatedCustomersGuestJourneysOut = updatedCustomersGuestJourneysOut;
    }
    public UpdatedCustomersGuestJourneysOut UpdatedCustomersGuestJourneysOut { get; set; }
}
public class UpdatedCustomersGuestJourneysOut
{
    public int Id { get; set; }
    public byte? JourneyStep { get; set; }
    public string? Name { get; set; }
    public string? WhatsappTemplateName { get; set; }   
    public byte? SendType { get; set; }
    public byte? TimingOption1 { get; set; }
    public byte? TimingOption2 { get; set; }
    public byte? TimingOption3 { get; set; }
    public int? Timing { get; set; }
    public string? NotificationDays { get; set; }
    public TimeSpan? NotificationTime { get; set; }
    public int? GuestJourneyMessagesTemplateId { get; set; }
    public string? TempletMessage { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? VonageTemplateStatus { get; set; }
    public string? Buttons { get; set; }
}