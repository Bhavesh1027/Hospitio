namespace HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;

public class GetTicketsWithFiltersIn
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int? CategoryId { get; set; } = 0;
    public byte? Status { get; set; } = 0;
    public byte? Priority { get; set; } = 0;
    public int? CustomerId { get; set; } = 0;
    public int? CSAgentId { get; set; } = 0;
    public DateTime? FromCreate { get; set; } = null;
    public DateTime? ToCreate { get; set; } = null;
    public DateTime? FromClose { get; set; } = null;
    public DateTime? ToClose { get; set; } = null;
    public byte ShortBy { get; set; }
    public byte CreatedFrom { get; set; }
    public bool ApplyPagination { get; set; }
}


