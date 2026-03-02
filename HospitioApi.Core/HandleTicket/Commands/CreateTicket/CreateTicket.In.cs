namespace HospitioApi.Core.HandleTicket.Commands.CreateTicket;
public class CreateTicketIn
{
    public int? CustomerId { get; set; }

    public string? Title { get; set; }

    public string? Details { get; set; }

    /// <summary>
    ///  1. High, 2. Medium, 3. Low
    /// </summary>
    public byte? Priority { get; set; }

    public DateTime? Duedate { get; set; }
    public int? CSAgentId { get; set; }


    /// <summary>
    /// 1. Pending, 2. Assigned, 3. Closed
    /// </summary>
    public byte? Status { get; set; }
    //public DateTime? CloseDate { get; set; }

}
