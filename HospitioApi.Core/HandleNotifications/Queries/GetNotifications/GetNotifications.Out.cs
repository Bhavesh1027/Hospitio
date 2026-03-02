using HospitioApi.Shared;

namespace HospitioApi.Core.HandleNotifications.Queries.GetNotifications;

public class GetNotificationsOut : BaseResponseOut
{
    public GetNotificationsOut(string message, List<NotificationOut> notificationOut) : base(message)
    {
        NotificationOut = notificationOut;
    }
    public List<NotificationOut> NotificationOut { get; set; } = new();
}

public class NotificationOut
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? FilteredCount { get; set; }
    public string? MessageType { get; set; }
}
