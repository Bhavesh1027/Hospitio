using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserByCustomerId;

public class GetCustomerUserByCustomerIdOut : BaseResponseOut
{
    public GetCustomerUserByCustomerIdOut(string message, List<CustomerUsersByCustomerIdOut> customerStaffsByCustomerIdOut) : base(message)
    {
        CustomerStaffsByCustomerIdOut = customerStaffsByCustomerIdOut;
    }
    public List<CustomerUsersByCustomerIdOut> CustomerStaffsByCustomerIdOut { get; set; } = new List<CustomerUsersByCustomerIdOut>();
}
public class CustomerUsersByCustomerIdOut
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
}