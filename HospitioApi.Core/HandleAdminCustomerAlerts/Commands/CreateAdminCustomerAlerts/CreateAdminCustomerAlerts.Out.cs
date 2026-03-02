using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.CreateAdminCustomerAlerts;

public class CreateAdminCustomerAlertsOut : BaseResponseOut
{
    public CreateAdminCustomerAlertsOut(string message, CreatedAdminCustomerAlertsOut createdAdminCustomerAlertsOut) : base(message)
    {
        CreatedAdminCustomerAlertsOut = createdAdminCustomerAlertsOut;
    }
    public CreatedAdminCustomerAlertsOut CreatedAdminCustomerAlertsOut { get; set; }
}
public class CreatedAdminCustomerAlertsOut
{
    public int Id { get; set; }
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }
    public bool? IsActive { get; set; }
}