namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;

public class CreateAdminCustomerAlertsIn
{
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }
    public bool? IsActive { get; set; }
}
