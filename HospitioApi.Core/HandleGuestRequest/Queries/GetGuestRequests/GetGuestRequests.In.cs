namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;

public class GetGuestRequestsIn
{
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
}
