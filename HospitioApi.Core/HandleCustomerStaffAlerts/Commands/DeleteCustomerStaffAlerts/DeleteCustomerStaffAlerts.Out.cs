using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.DeleteCustomerStaffAlerts;

public class DeleteCustomerStaffAlertsOut : BaseResponseOut
{
    public DeleteCustomerStaffAlertsOut(string message, DeletedCustomerStaffAlertsOut deletedCustomerStaffAlertsOut) : base(message)
    {
        DeletedCustomerStaffAlertsOut = deletedCustomerStaffAlertsOut;
    }
    public DeletedCustomerStaffAlertsOut DeletedCustomerStaffAlertsOut { get; set; }
}

public class DeletedCustomerStaffAlertsOut
{
    public int Id { get; set; }
}
