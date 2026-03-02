using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Commands.DeleteCustomerGuestAlerts;

public class DeleteCustomerGuestAlertsOut : BaseResponseOut
{
    public DeleteCustomerGuestAlertsOut(string message, DeletedCustomerGuestAlertsOut deletedCustomerGuestAlertsOut) : base(message)
    {

        DeletedCustomerGuestAlertsOut = deletedCustomerGuestAlertsOut;
    }
    public DeletedCustomerGuestAlertsOut DeletedCustomerGuestAlertsOut { get; set; }
}
public class DeletedCustomerGuestAlertsOut
{
    public int Id { get; set; }
}
