using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;

public class GetCustomerGuestByIdOut : BaseResponseOut
{
    public GetCustomerGuestByIdOut(string message, CustomerGuestByIdOut customerGuestByIdOut) : base(message)
    {
        CustomerGuestByIdOut = customerGuestByIdOut;
    }
    public CustomerGuestByIdOut CustomerGuestByIdOut { get; set; }
}
public class CustomerGuestByIdOut
{
    public int Id { get; set; }
    public int? CustomerReservationId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Picture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? Language { get; set; }
    public string? IdProof { get; set; }
    public string? IdProofType { get; set; }
    public string? IdProofNumber { get; set; }
    public string? BlePinCode { get; set; }
    public string? Pin { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public string? ArrivalFlightNumber { get; set; }
    public string? DepartureAirline { get; set; }
    public string? DepartureFlightNumber { get; set; }
    public string? Signature { get; set; }
    public string? RoomNumber { get; set; }
    public bool? TermsAccepted { get; set; }
    public byte? FirstJourneyStep { get; set; }
    public int? Rating { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Vat { get; set; }
    public bool? IsActive { get; set; }
    public string? BookingChannel { get; set; }
    public DateTime? DepartingFlightDate { get; set; }
    public byte? AgeCategory { get; set; }
    public DateTime? CheckinDate { get;set; }
    public DateTime? CheckoutDate { get; set; }
    public string? ReservationNumber { get; set; }
    public bool? isCheckInCompleted { get; set; }
    public bool? isSkipCheckIn { get;set; }
}
