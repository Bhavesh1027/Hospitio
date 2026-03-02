using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Commands.DeleteAdminCustomerAlerts;

public class DeleteAdminCustomerAlertsOut : BaseResponseOut
{
    public DeleteAdminCustomerAlertsOut(string message, DeletedAdminCustomerAlertsOut deletedAdminCustomerAlertsOut) : base(message)
    {

        DeletedAdminCustomerAlertsOut = deletedAdminCustomerAlertsOut;
    }
    public DeletedAdminCustomerAlertsOut DeletedAdminCustomerAlertsOut { get; set; }
}
public class DeletedAdminCustomerAlertsOut
{
    public int Id { get; set; }
}