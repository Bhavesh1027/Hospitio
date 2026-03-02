namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;

public class GetCustomersMainInfoIn
{
    //public string SearchColumn { get; set; } = string.Empty;
    public string SearchValue { get; set; } = string.Empty;
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public string? AlphabetsStartsWith { get;set; }
}
