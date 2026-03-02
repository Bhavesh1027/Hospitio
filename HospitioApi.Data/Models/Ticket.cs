using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class Ticket : AuditableCreatedFrom
{
    public Ticket()
    {
        TicketReplies = new HashSet<TicketReply>();
    }

    public int? CustomerId { get; set; }
    [MaxLength(100)]
    public string? Title { get; set; }
    public string? Details { get; set; }
    /// <summary>
    ///  1. High, 2. Medium, 3. Low
    /// </summary>
    [DefaultValue(3)]
    public byte? Priority { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? Duedate { get; set; }
    public int? TicketCategoryId { get; set; }
    public int? CSAgentId { get; set; }

    public int? GroupId { get; set; }
    /// <summary>
    /// 1. Pending, 2. Assigned, 3. Closed
    /// </summary>
    [DefaultValue(1)]
    public byte? Status { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? CloseDate { get; set; }
    public virtual User? Csagent { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual TicketCategory? TicketCategory { get; set; }
    public virtual ICollection<TicketReply> TicketReplies { get; set; }

    public virtual Group? Group { get; set; }

}
