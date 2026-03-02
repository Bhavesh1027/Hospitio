namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendWelcomeMessage;

public class SendWelcomeMessageIn
{
    public int? GuestId { get; set; }
    public string? GuestPortal { get; set; }  
}
