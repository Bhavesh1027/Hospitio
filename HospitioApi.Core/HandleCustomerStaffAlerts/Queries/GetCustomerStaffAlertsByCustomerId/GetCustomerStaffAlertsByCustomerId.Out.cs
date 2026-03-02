using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsByCustomerId;

public class GetCustomerStaffAlertsByCustomerIdOut : BaseResponseOut
{
    public GetCustomerStaffAlertsByCustomerIdOut(string message, List<CustomerStaffAlertsByCustomerIdOut> customerStaffAlertsByCustomerIdOut) : base(message)
    {
        CustomerStaffAlertsByCustomerIdOut = customerStaffAlertsByCustomerIdOut;
    }
    public List<CustomerStaffAlertsByCustomerIdOut> CustomerStaffAlertsByCustomerIdOut { get; set; } = new List<CustomerStaffAlertsByCustomerIdOut>();
}
public class CustomerStaffAlertsByCustomerIdOut
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
