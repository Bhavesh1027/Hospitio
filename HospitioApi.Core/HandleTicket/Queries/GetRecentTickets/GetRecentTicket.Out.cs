using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Queries.GetRecentTickets;

public class GetRecentTicketOut: BaseResponseOut
{
    public GetRecentTicketOut(string message, List<GetRecentTicketsResponseOut> GetTicketsResponseOut) : base(message)
    {
        getTicketsResponseOut = GetTicketsResponseOut;
    }

    public List<GetRecentTicketsResponseOut> getTicketsResponseOut { get; set; } = new List<GetRecentTicketsResponseOut>();
}
public class GetRecentTicketsResponseOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public byte? Priority { get; set; }
    public DateTime? Duedate { get; set; }
    public int? TicketCategoryId { get; set; }
    public int? CSAgentId { get; set; }
    public byte? Status { get; set; }
    public DateTime? CloseDate { get; set; }
    public byte? CreatedFrom { get; set; }
    public DateTime? CreatedAt { get; set; }
}