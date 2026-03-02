namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;

public class GetCustomerGuestsIn
{
    public string SearchValue { get; set; } = string.Empty;
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
}
