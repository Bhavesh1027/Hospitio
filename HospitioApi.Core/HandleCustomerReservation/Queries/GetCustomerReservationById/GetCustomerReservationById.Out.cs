using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationById;

public class GetCustomerReservationByIdOut : BaseResponseOut
{
    public GetCustomerReservationByIdOut(string message, CustomerReservationByIdOut customerReservationByIdOut) : base(message)
    {
        CustomerReservationByIdOut = customerReservationByIdOut;
    }
    public CustomerReservationByIdOut CustomerReservationByIdOut { get; set; }
}
public class CustomerReservationByIdOut
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
