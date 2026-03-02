using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;

public class GetCustomerGuestsOut : BaseResponseOut
{
    public GetCustomerGuestsOut(string message, List<CustomerGuestsOut> customerGuestsOut) : base(message)
    {
        CustomerGuestsOut = customerGuestsOut;
    }
    public List<CustomerGuestsOut> CustomerGuestsOut { get; set; }
}
public class CustomerGuestsOut
{
    public int Id { get; set; }
    public int? CustomerReservationId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? RoomNumber { get; set; }
    public int GuestStatus { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get;set; }
    public int FilteredCount { get; set; }
    public string? GuestToken { get;set; }
}
