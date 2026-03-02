using HospitioApi.Shared;

namespace HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunicationByReservationId;
public class GetCustomerCommunicationByReservationOut : BaseResponseOut
{
    public GetCustomerCommunicationByReservationOut(string message, GetCustomerCommunicationByReservationIdResponseOut getCustomerCommunicationByReservationIdResponseOut) : base(message)
    {
        GetCustomerCommunicationByReservationIdResponseOut = getCustomerCommunicationByReservationIdResponseOut;
    }
    public GetCustomerCommunicationByReservationIdResponseOut GetCustomerCommunicationByReservationIdResponseOut { get; set; }
}
public class GetCustomerCommunicationByReservationIdResponseOut
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? language { get; set; }
    public string? RoomNumber { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
}
