namespace HospitioApi.Core.Services.Chat.Models.Chat;

public class GuestDetails
{
    public int ChannelId { get; set; }
    public int GuestId { get; set; }
    public string? GuestFirstName { get; set; }
    public string? GuestLastName { get; set; }
    public string? CustomerFirstName { get; set; }
    public string? CustomerLastName { get; set; }
    public string? GuestProfile { get; set; }
    public string? CustomerProfile { get; set; }
}
