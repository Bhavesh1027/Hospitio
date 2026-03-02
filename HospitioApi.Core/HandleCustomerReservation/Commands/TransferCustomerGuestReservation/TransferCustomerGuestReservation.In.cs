namespace HospitioApi.Core.HandleCustomerReservation.Commands.TransferCustomerGuestReservation;

public class TransferCustomerGuestReservationIn
{
    public string ReservationNumber { get; set; }
    public Guid LocationCode { get; set; }
    public Guid UserCode { get; set; }
    /// <summary>
    /// Use the same location code if reservation only needs to be moved to a different date period.
    /// </summary>
    public Guid NewLocationCode { get; set; }
    /// <summary>
    /// Departure Date.
    /// </summary>
    /// <example>dd/MM/yyyy</example>
    public string ArrivalDate { get; set; }
    /// <summary>
    /// Departure time
    /// </summary>
    /// <example>HH:mm</example>
    public string ArrivalTime { get; set; }
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
