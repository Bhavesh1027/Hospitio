namespace HospitioApi.Core.HandleCustomerReservation.Commands.ExtendCustomerGuestReservation;

public class ExtendCustomerGuestReservationIn
{
    public string ReservationNumber { get; set; }
    public Guid LocationCode { get; set; }
    public Guid UserCode { get; set; }
    /// <summary>
    /// Departure Date
    /// </summary>
    /// <example>dd/MM/yyyy</example>
    public string DepartureDate { get; set; }
    /// <summary>
    /// Departure time
    /// </summary>
    /// <example>HH:mm</example>
    public string DepartureTime { get; set; }
}
