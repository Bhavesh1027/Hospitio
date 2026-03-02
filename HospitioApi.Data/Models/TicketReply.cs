namespace HospitioApi.Data.Models;

public partial class TicketReply : AuditableCreatedFrom
{
    public string? Reply { get; set; }
    public int? TicketId { get; set; }
    public bool? IsRead { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
