using HospitioApi.Shared;

namespace HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunication;
public class GetCustomerCommunicationOut : BaseResponseOut
{
    public GetCustomerCommunicationOut(string message, List<CustomerCommunicationOut> customerCommunicationOut) : base(message)
    {
        CustomerCommunicationOut = customerCommunicationOut;
    }
    public List<CustomerCommunicationOut> CustomerCommunicationOut { get; set; }
}
public class CustomerCommunicationOut
{
    public int? Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerReservationId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Picture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? language { get; set; }
    public string? UserType { get; set; }
    public string? ChatType { get; set; }
}
