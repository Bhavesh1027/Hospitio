using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HospitioApi.Data.Models;

public partial class CustomerReservation : Auditable
{
    public CustomerReservation()
    {
        CustomerGuests = new HashSet<CustomerGuest>();
    }

    public int? CustomerId { get; set; }
    [MaxLength(32)]
    public string? Uuid { get; set; }
    [MaxLength(100)]
    public string? ReservationNumber { get; set; }
    [MaxLength(20)]
    public string? Source { get; set; }
    public int? NoOfGuestAdults { get; set; }
    public int? NoOfGuestChildrens { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CheckinDate { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CheckoutDate { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<CustomerGuest> CustomerGuests { get; set; }
}
