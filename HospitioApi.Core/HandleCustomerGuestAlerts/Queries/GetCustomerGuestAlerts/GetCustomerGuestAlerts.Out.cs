using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlerts;

public class GetCustomerGuestAlertsOut : BaseResponseOut
{
    public GetCustomerGuestAlertsOut(string message, List<CustomerGuestAlertsOut> customerGuestAlertsOut) : base(message)
    {
        CustomerGuestAlertsOut = customerGuestAlertsOut;
    }
    public List<CustomerGuestAlertsOut> CustomerGuestAlertsOut { get; set; } = new List<CustomerGuestAlertsOut>();
}

public class CustomerGuestAlertsOut
{
    public int Id { get; set; }
    public string? OfficeHoursMsg { get; set; }
    public int? OfficeHoursMsgWaitTimeInMinutes { get; set; }
    public string? OfflineHourMsg { get; set; }
    public int? OfflineHoursMsgWaitTimeInMinutes { get; set; }
    public bool? ReplyAtDiffPeriod { get; set; }
    public bool? IsActive { get; set; }
}
