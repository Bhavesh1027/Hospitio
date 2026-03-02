namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendGuestPdfMail;

public class SendGuestPdfMailIn
{
    public string? GuestId { get; set; }
    public string? CustomerId { get; set; }
    public string? HtmlContent { get; set; }
}
