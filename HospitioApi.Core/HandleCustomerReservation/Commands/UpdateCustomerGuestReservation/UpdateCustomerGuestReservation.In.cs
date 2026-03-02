namespace HospitioApi.Core.HandleCustomerReservation.Commands.UpdateCustomerGuestReservation;

public class UpdateCustomerGuestReservationIn
{
    public string ReservationNumber { get; set; }
    public Guid LocationCode { get; set; }
    public Guid UserCode { get; set; }
    /// <summary>
    /// Reservation number
    /// </summary>
    /// <example>Mr.|Mrs.|Ms.|Dear</example>
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    /// <summary>
    /// Reservation number
    /// </summary>
    /// <example>+(CountryCode)(Rest) -> +306973546799</example>
    public string PhoneNumber { get; set; }
}
