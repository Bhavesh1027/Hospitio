using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsById;

public class GetCustomerStaffAlertsByIdOut : BaseResponseOut
{
    public GetCustomerStaffAlertsByIdOut(string message, CustomerStaffAlertsByIdOut customerStaffAlertsByIdOut) : base(message)
    {
        CustomerStaffAlertsByIdOut = customerStaffAlertsByIdOut;
    }
    public CustomerStaffAlertsByIdOut CustomerStaffAlertsByIdOut { get; set; }
}

public class CustomerStaffAlertsByIdOut
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Platfrom { get; set; } = string.Empty;
    public string PhoneCountry { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? WaitTimeInMintes { get; set; }
    public bool? IsActive { get; set; }
}
