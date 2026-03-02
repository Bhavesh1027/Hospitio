using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneys;

public class GetCustomersGuestJourneysOut : BaseResponseOut
{
    public GetCustomersGuestJourneysOut(string message, List<CustomersGuestJourneysOut> customersGuestJourneysOut) : base(message)
    {
        CustomersGuestJourneysOut = customersGuestJourneysOut;
    }
    public List<CustomersGuestJourneysOut> CustomersGuestJourneysOut { get; set; } = new List<CustomersGuestJourneysOut>();
}
public class CustomersGuestJourneysOut
{
    public int Id { get; set; }
    public int CutomerId { get; set; }
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
    public string? Buttons { get; set; }

    public string? VonageTemplateId { get; set; }

    public string? VonageTemplateStatus { get; set; }
    public bool? IsActive { get; set; }
}