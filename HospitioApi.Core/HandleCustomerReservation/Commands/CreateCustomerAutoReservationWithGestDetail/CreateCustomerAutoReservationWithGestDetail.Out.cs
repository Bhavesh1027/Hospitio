using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetail;

public class CreateCustomerAutoReservationWithGestDetailOut : BaseResponseOut
{
    public CreateCustomerAutoReservationWithGestDetailOut(string message, CreatedCustomerReservationOut createdCustomerReservationOut) : base(message)
    {
        CreatedCustomerReservationOut = createdCustomerReservationOut;
    }
    public CreatedCustomerReservationOut CreatedCustomerReservationOut { get; set; }
}
public class CreatedCustomerReservationOut
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
