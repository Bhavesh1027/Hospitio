using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendWelcomeMessage;

public class SendWelcomeMessageOut : BaseResponseOut
{
    public SendWelcomeMessageOut(string message, SendWelcomeMessageGuestOut createdCustomerGuestOut) : base(message)
    {
        CreatedCustomerGuestOut = createdCustomerGuestOut;
    }
    public SendWelcomeMessageGuestOut CreatedCustomerGuestOut { get; set; }
}
public class SendWelcomeMessageGuestOut
{
    public int? GuestId { get; set; }
    public string? GuestPortal { get; set; }
}