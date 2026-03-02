namespace HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerReservation;

public class UpdateCustomerReservationIn
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
