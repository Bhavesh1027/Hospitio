using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.ExtendCustomerGuestReservation;

public class ExtendCustomerGuestReservationOut : BaseResponseOut
{
    public ExtendCustomerGuestReservationOut(string message, ExtendedCustomerReservationOut updatedCustomerReservationOut) : base(message)
    {
        UpdatedCustomerReservationOut = updatedCustomerReservationOut;
    }
    public ExtendedCustomerReservationOut UpdatedCustomerReservationOut { get; set; }
}
public class ExtendedCustomerReservationOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Uuid { get; set; }
    public string? ReservationNumber { get; set; }
    public string? Source { get; set; }
    public int? NoOfGuestAdults { get; set; }
    public int? NoOfGuestChilderns { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public bool? IsActive { get; set; }
}