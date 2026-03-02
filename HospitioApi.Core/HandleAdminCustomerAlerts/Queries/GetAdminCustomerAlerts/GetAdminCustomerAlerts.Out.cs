using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Queries.GetAdminCustomerAlerts;

public class GetAdminCustomerAlertsOut : BaseResponseOut
{
    public GetAdminCustomerAlertsOut(string message, List<AdminCustomerAlertsOut> adminCustomerAlertsOut) : base(message)
    {
        AdminCustomerAlertsOut = adminCustomerAlertsOut;
    }
    public List<AdminCustomerAlertsOut> AdminCustomerAlertsOut { get; set; } = new List<AdminCustomerAlertsOut>();
}
public class AdminCustomerAlertsOut
{
    public int Id { get; set; }
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }
    public bool? IsActive { get; set; }
}