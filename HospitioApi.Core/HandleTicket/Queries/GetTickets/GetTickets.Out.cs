using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Queries.GetTickets;

public class GetTicketsOut : BaseResponseOut
{
    public GetTicketsOut(string message, List<GetTicketsResponseOut> GetTicketsResponseOut) : base(message)
    {
        getTicketsResponseOut = GetTicketsResponseOut;
    }

    public List<GetTicketsResponseOut> getTicketsResponseOut { get; set; } = new List<GetTicketsResponseOut>();

}

public class GetTicketsResponseOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public byte? Priority { get; set; }
    public DateTime? Duedate { get; set; }
    public int? TicketCategoryId { get; set; }
    public string? TicketCategoryName { get; set; }
    public int? CSAgentId { get; set; }
    public string? CSAgentName { get; set; }
    public byte? Status { get; set; }
    public DateTime? CloseDate { get; set; }
    public byte? CreatedFrom { get; set; }
}

