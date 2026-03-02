using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class TicketCategory : Auditable
{
    public TicketCategory()
    {
        Tickets = new HashSet<Ticket>();
    }

    [MaxLength(50)]
    public string? CategoryName { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; }
}
