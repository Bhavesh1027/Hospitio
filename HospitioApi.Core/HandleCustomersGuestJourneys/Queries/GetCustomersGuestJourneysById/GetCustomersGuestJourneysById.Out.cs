using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;

public class GetCustomersGuestJourneysByIdOut : BaseResponseOut
{
    public GetCustomersGuestJourneysByIdOut(string message, CustomersGuestJourneysByIdOut customersGuestJourneysByIdOut) : base(message)
    {
        CustomersGuestJourneysByIdOut = customersGuestJourneysByIdOut;
    }
    public CustomersGuestJourneysByIdOut CustomersGuestJourneysByIdOut { get; set; }
}
public class CustomersGuestJourneysByIdOut
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
}