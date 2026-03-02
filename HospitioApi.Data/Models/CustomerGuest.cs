using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class CustomerGuest : Auditable
{
    public CustomerGuest()
    {
        GuestRequests = new HashSet<GuestRequest>();
        TaxiTransferGuestRequests = new HashSet<TaxiTransferGuestRequest>();
    }

    public int? CustomerReservationId { get; set; }
    [MaxLength(50)]
    public string? GRBuyerId { get; set; }
    [MaxLength(50)]
    public string? GRAdminBuyerId { get; set; }
    [MaxLength(50)]
    public string? Title { get; set; }

    [MaxLength(50)]
    public string? Firstname { get; set; }
    [MaxLength(50)]
    public string? Lastname { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    [MaxLength(500)]
    public string? Picture { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [MaxLength(3)]
    public string? Country { get; set; }
    [MaxLength(10)]
    public string? Language { get; set; }
    [MaxLength(500)]
    public string? IdProof { get; set; }
    [MaxLength(50)]
    public string? IdProofType { get; set; }
    [MaxLength(50)]
    public string? IdProofNumber { get; set; }
    [MaxLength(50)]
    public string? BlePinCode { get; set; }
    [MaxLength(10)]
    public string? Pin { get; set; }
    [MaxLength(100)]
    public string? Street { get; set; }
    [MaxLength(5)]
    public string? StreetNumber { get; set; }
    [MaxLength(20)]
    public string? City { get; set; }
    [MaxLength(10)]
    public string? Postalcode { get; set; }
    [MaxLength(20)]
    public string? ArrivalFlightNumber { get; set; }
    [MaxLength(20)]
    public string? DepartureAirline { get; set; }
    [MaxLength(20)]
    public string? DepartureFlightNumber { get; set; }
    public string? Signature { get; set; }
    [MaxLength(150)]
    public string? RoomNumber { get; set; }
    [DefaultValue(1)]
    public bool? TermsAccepted { get; set; }
    /// <summary>
    ///  1: Automatic, 2: Post Booking, 3: Online Check in, 4: Pre-Arrival, 5: In-House, 6: Post-Arrival
    /// </summary>
    [DefaultValue(1)]
    public byte? FirstJourneyStep { get; set; }
    public int? Rating { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? DateOfBirth { get; set; }
    [MaxLength(100)]
    public string? Vat { get; set; }

    /// <summary>
    ///  1: Adult>18, 2: Child<18
    /// </summary>
    public byte? AgeCategory { get; set; }
    public bool IsCoGuest { get; set; }

    [MaxLength(50)]
    public string? BookingChannel { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DepartingFlightDate { get; set; }
    public Guid? CustomerRoomGuid { get; set; }
    public string? GuestToken { get; set; }
    public string? GuestURL { get; set; }
    public bool isSkipCheckIn { get; set; }
    public bool isCheckInCompleted { get; set; }
    public string? AppAccessCode { get; set; }
    public int? KeyId { get; set; }
    public virtual CustomerReservation? CustomerReservation { get; set; }
    public virtual ICollection<GuestRequest> GuestRequests { get; set; }
    public virtual ICollection<TaxiTransferGuestRequest> TaxiTransferGuestRequests { get; set; }


}
