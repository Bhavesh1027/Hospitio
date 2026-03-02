using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;

public class GetTicketsWithFiltersOut : BaseResponseOut
{
    public GetTicketsWithFiltersOut(string message, List<GetTicketsWithFiltersResponseOut> getTicketsWithFiltersResponseOut) : base(message)
    {
        GetTicketsWithFiltersResponseOut = getTicketsWithFiltersResponseOut;
    }

    public List<GetTicketsWithFiltersResponseOut> GetTicketsWithFiltersResponseOut { get; set; } = new List<GetTicketsWithFiltersResponseOut>();

}

public class GetTicketsWithFiltersResponseOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? BusinessName { get; set; }
    public string? CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public byte? Priority { get; set; }
    public DateTime? Duedate { get; set; }
    public string? TicketCategoryName { get; set; }
    public string? CSAgentName { get; set; }
    public byte? Status { get; set; }
    public DateTime? CloseDate { get; set; }
    public byte? CreatedFrom { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? MaxCreatedDate { get; set; }
    public string? ProfilePicture { get; set; }
    public int FilteredCount { get; set; }
    public int TotalUnReadCount { get; set; }
}

