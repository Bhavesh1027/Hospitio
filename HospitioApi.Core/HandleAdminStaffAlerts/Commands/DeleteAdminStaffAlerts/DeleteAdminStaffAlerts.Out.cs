using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.DeleteAdminStaffAlerts;

public class DeleteAdminStaffAlertsOut : BaseResponseOut
{
    public DeleteAdminStaffAlertsOut(string message, DeletedAdminStaffAlertsOut deletedAdminStaffAlertsOut) : base(message)
    {
        DeletedAdminStaffAlertsOut = deletedAdminStaffAlertsOut;
    }
    public DeletedAdminStaffAlertsOut DeletedAdminStaffAlertsOut { get; set; }
}
public class DeletedAdminStaffAlertsOut
{
    public int Id { get; set; }
}
