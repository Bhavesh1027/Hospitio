namespace HospitioApi.Core.HandleNotifications.Queries.GetNotifications;

public class GetNotificationsIn
{
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }

    public int? UserId { get; set; }
    public int? UserType { get; set; }

}
