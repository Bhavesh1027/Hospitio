namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;

public class UpdateCustomerGuestAlertsIn
{
    public int Id { get; set; }
    public string? OfficeHoursMsg { get; set; }
    public int? OfficeHoursMsgWaitTimeInMinutes { get; set; }
    public string? OfflineHourMsg { get; set; }
    public int? OfflineHoursMsgWaitTimeInMinutes { get; set; }
    public bool? ReplyAtDiffPeriod { get; set; }
    public bool? IsActive { get; set; }
}
