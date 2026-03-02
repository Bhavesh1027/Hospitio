using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Commands.CreateCustomerStaffAlerts;

public class CreateCustomerStaffAlertsOut : BaseResponseOut
{
    public CreateCustomerStaffAlertsOut(string message, CreatedCustomerStaffAlertsOut createdCustomerStaffAlertsOut) : base(message)
    {
        CreatedCustomerStaffAlertsOut = createdCustomerStaffAlertsOut;
    }
    public CreatedCustomerStaffAlertsOut CreatedCustomerStaffAlertsOut { get; set; }
}

public class CreatedCustomerStaffAlertsOut
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
