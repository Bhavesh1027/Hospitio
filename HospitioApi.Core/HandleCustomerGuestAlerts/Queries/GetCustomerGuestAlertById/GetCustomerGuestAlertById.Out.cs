using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertById;

public class GetCustomerGuestAlertByIdOut : BaseResponseOut
{
    public GetCustomerGuestAlertByIdOut(string message, CustomerGuestAlertByIdOut customerGuestAlertByIdOut) : base(message)
    {
        CustomerGuestAlertByIdOut = customerGuestAlertByIdOut;
    }
    public CustomerGuestAlertByIdOut CustomerGuestAlertByIdOut { get; set; }
}
public class CustomerGuestAlertByIdOut
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
