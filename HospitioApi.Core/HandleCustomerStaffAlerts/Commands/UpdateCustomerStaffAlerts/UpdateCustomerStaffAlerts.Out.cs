using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.UpdateCustomerStaffAlerts;

public class UpdateCustomerStaffAlertsOut : BaseResponseOut
{
    public UpdateCustomerStaffAlertsOut(string message, UpdatedCustomerStaffAlertsOut updatedCustomerStaffAlertsOut) : base(message)
    {
        UpdatedCustomerStaffAlertsOut = updatedCustomerStaffAlertsOut;
    }
    public UpdatedCustomerStaffAlertsOut UpdatedCustomerStaffAlertsOut { get; set; }
}

public class UpdatedCustomerStaffAlertsOut
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Platfrom { get; set; } = string.Empty;
    public string PhoneCountry { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? WaitTimeInMintes { get; set; }
    public bool? IsActive { get; set; }
    public string? Msg { get; set; }
    public int? CustomerUserId { get; set; }

}
