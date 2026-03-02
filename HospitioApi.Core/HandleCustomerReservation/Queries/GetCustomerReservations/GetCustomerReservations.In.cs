namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;

public class GetCustomerReservationsIn
{
    public int CustomerId { get; set; }
    public string SearchColumn { get; set; } = string.Empty;
    public string SearchValue { get; set; } = string.Empty;
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
}
