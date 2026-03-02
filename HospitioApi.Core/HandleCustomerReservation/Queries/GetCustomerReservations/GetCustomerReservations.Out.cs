using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;

public class GetCustomerReservationsOut : BaseResponseOut
{
    public GetCustomerReservationsOut(string message, List<CustomerReservationsOut> customerReservationsOut) : base(message)
    {
        CustomerReservationsOut = customerReservationsOut;
    }
    public List<CustomerReservationsOut> CustomerReservationsOut { get; set; }
}
public class CustomerReservationsOut
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
