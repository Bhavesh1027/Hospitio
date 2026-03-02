namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;

public class CreateCustomersGuestJourneysIn
{
    public int CustomerId { get; set; }
    public byte? JourneyStep { get; set; }
    public string? Name { get; set; }
    public byte? SendType { get; set; }
    public byte? TimingOption1 { get; set; }
    public byte? TimingOption2 { get; set; }
    public byte? TimingOption3 { get; set; }
    public int? Timing { get; set; }
    public string? NotificationDays { get; set; }
    public TimeSpan? NotificationTime { get; set; }
    public int? GuestJourneyMessagesTemplateId { get; set; }
    public string? TempletMessage { get; set; }
    public List<Button>? Buttons { get; set; } // List of buttons to be added
}
public class Button
{
    public string? type { get; set; } // Button type, e.g., "URL" or "PHONE_NUMBER"
    public string? text { get; set; } // Button text
    public string? value { get; set; } // URL (if it's a URL button) Phone number (if it's a phone number button)
}
