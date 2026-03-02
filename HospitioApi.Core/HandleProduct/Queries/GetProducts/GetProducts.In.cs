namespace HospitioApi.Core.HandleProduct.Queries.GetProducts;

public class GetProductsIn
{
    public string SearchValue { get; set; } = string.Empty;
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
}


