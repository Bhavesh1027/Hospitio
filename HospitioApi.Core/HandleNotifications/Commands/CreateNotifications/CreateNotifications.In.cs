namespace HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;

public class CreateNotificationsIn
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? ProductId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public int? CustomerId { get; set; }
    public int? CurrentUserType { get; set; }
    public int? UserId { get; set; }
}
