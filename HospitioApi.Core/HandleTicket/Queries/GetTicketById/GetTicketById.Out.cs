using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Queries.GetTicketById;

public class GetTicketByIdOut : BaseResponseOut
{
    public GetTicketByIdOut(string message, List<GetTicketByIdResponseOut> getTicketByIdResponseOut) : base(message)
    {
        GetTicketByIdResponseOut = getTicketByIdResponseOut;
    }


    public List<GetTicketByIdResponseOut> GetTicketByIdResponseOut { get; set; } = new List<GetTicketByIdResponseOut>();

}

public class GetTicketByIdResponseOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public byte? Priority { get; set; }
    public DateTime? Duedate { get; set; }
    public int? CSAgentId { get; set; }
    public string? CSAgentName { get; set; }
    public byte? Status { get; set; }
    public DateTime? CloseDate { get; set; }
    public byte? CreatedFrom { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GetTicketByIdRepliesResponseOut>? Replies { get; set; }
}


public class GetTicketByIdRepliesResponseOut
{
    public int Id { get; set; }
    public int? TicketId { get; set; }
    public string? Reply { get; set; }
    public string? UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public byte? CreatedFrom { get; set; }
}
