using HospitioApi.Shared;
using System.Text.Json.Serialization;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersByGuIds;

public class GetCustomersByGuIdsOut: BaseResponseOut
{
    public GetCustomersByGuIdsOut(string message, List<Customers> customers) : base(message)
    {
        GetCustomers = customers;
    }

    public List<Customers> GetCustomers { get; set; } = new List<Customers>();
}
public class Customers
{
    [JsonPropertyName("userCode")]
    public Guid UserCode { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("mobile")]
    public string Mobile { get; set; }
}