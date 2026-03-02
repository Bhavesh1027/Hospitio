using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;

public class CreateCustomersGuestJourneysOut : BaseResponseOut
{
    public CreateCustomersGuestJourneysOut(string message, CreatedCustomersGuestJourneysOut createdCustomersGuestJourneysOut) : base(message)
    {
        CreatedCustomersGuestJourneysOut = createdCustomersGuestJourneysOut;
    }
    public CreatedCustomersGuestJourneysOut CreatedCustomersGuestJourneysOut { get; set; }
}
public class CreatedCustomersGuestJourneysOut
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
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
    public bool? IsActive { get; set; }
    public string?  Buttons { get; set; }
}