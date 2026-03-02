using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;

public class GetCustomerReservationByReservationNumberOut: BaseResponseOut
{
    public GetCustomerReservationByReservationNumberOut(string message, CustomerReservationByNumberOut customerReservation) : base(message)
    {
        customerReservationByNumberOut = customerReservation;
    }
    public CustomerReservationByNumberOut customerReservationByNumberOut { get; set; }
}
public class CustomerReservationByNumberOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Uuid { get; set; }
    public string? ReservationNumber { get; set; }
    public string? Source { get; set; }
    public int? NoOfGuestAdults { get; set; }
    public int? NoOfGuestChildrens { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public bool? IsActive { get; set; }
}
