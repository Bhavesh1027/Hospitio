namespace HospitioApi.Core.HandleCustomerReservation.Commands.CreateCustomerGuestReservation;

public class CreateCustomerGuestReservationIn
{
    public string ReservationNumber { get; set; }
    public Guid LocationCode { get; set; }
    public Guid UserCode { get; set; }
    /// <summary>
    /// Reservation number
    /// </summary>
    /// <example>Mr.|Mrs.|Ms.|Dear </example>
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    /// <summary>
    /// Reservation number
    /// </summary>
    /// <example>+(CountryCode)(Rest) -> +306973546799</example>
    public string PhoneNumber { get; set; }
    /// <summary>
    /// Arrival Date
    /// </summary>
    /// <example>dd/MM/yyyy</example>
    public string ArrivalDate { get; set; }
    /// <summary>
    /// Departure Date
    /// </summary>
    /// <example>dd/MM/yyyy</example>
    public string DepartureDate { get; set; }
    /// <summary>
    /// Arrival time
    /// </summary>
    /// <example>HH:mm</example>
    public string ArrivalTime { get; set; }
    /// <summary>
    /// Departure time
    /// </summary>
    /// <example>HH:mm</example>
    public string DepartureTime { get; set; }
    public string? BluetoothPinCode { get; set; }
    public string? AppAccessCode { get; set; }
    public int? KeyId { get; set; }
}
