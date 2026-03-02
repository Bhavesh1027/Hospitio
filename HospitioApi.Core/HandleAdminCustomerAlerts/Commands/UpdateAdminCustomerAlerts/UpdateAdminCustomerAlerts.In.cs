namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;

public class UpdateAdminCustomerAlertsIn
{
    public int Id { get; set; }
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }
    public bool? IsActive { get; set; }
}
