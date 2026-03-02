using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.UpdateAdminCustomerAlerts;

public class UpdateAdminCustomerAlertsOut : BaseResponseOut
{
    public UpdateAdminCustomerAlertsOut(string message, UpdatedAdminCustomerAlertsOut updatedAdminCustomerAlertsOut) : base(message)
    {
        UpdatedAdminCustomerAlertsOut = updatedAdminCustomerAlertsOut;
    }
    public UpdatedAdminCustomerAlertsOut UpdatedAdminCustomerAlertsOut { get; set; }
}
public class UpdatedAdminCustomerAlertsOut
{
    public int Id { get; set; }
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }
    public bool? IsActive { get; set; }
}