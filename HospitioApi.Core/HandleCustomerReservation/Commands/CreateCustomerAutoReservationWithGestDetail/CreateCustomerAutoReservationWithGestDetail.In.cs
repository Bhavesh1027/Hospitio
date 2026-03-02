namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerAutoReservationWithGestDetail;

public class CreateCustomerAutoReservationWithGestDetailIn
{
    public int? CustomerId { get; set; }
    public string? Source { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public string? Title { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public byte? FirstJourneyStep { get; set; }
    public string? RoomNumber { get; set; }
}
