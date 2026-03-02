using HospitioApi.Shared;

namespace HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;

public class CreateNotificationsOut : BaseResponseOut
{
    public CreateNotificationsOut(string message, CreatedNotificationsOut createdNotificationsOut) : base(message)
    {
        CreatedNotificationsOut = createdNotificationsOut;
    }
    public CreatedNotificationsOut CreatedNotificationsOut { get; set; }
}

public class CreatedNotificationsOut
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? ProductId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class UserNotification
{
    public int UserId { get; set; }

    public int IsActive { get; set; }
}
