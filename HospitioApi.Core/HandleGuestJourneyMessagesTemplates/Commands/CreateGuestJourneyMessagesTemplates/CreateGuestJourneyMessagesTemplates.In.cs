namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;

public class CreateGuestJourneyMessagesTemplatesIn
{
    public int UserId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public byte? TempleteType { get; set; }
    public string? TempletMessage { get; set; }
    public bool? IsActive { get; set; }
    public List<Button>? Buttons { get; set; } // List of buttons to be added
}
public class Button
{
    public string? type { get; set; } // Button type, e.g., "URL" or "PHONE_NUMBER"
    public string? text { get; set; } // Button text
    public string? value { get; set; } // URL (if it's a URL button) Phone number (if it's a phone number button)
}
