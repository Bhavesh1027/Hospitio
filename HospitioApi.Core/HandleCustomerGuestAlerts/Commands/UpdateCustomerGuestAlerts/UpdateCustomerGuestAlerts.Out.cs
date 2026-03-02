using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.UpdateCustomerGuestAlerts;

public class UpdateCustomerGuestAlertsOut : BaseResponseOut
{
    public UpdateCustomerGuestAlertsOut(string message, UpdatedCustomerGuestAlertsOut updatedCustomerGuestAlertsOut) : base(message)
    {
        UpdatedCustomerGuestAlertsOut = updatedCustomerGuestAlertsOut;
    }
    public UpdatedCustomerGuestAlertsOut UpdatedCustomerGuestAlertsOut { get; set; }
}
public class UpdatedCustomerGuestAlertsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? OfficeHoursMsg { get; set; }
    public int? OfficeHoursMsgWaitTimeInMinutes { get; set; }
    public string? OfflineHourMsg { get; set; }
    public int? OfflineHoursMsgWaitTimeInMinutes { get; set; }
    public bool? ReplyAtDiffPeriod { get; set; }
    public bool? IsActive { get; set; }
}
