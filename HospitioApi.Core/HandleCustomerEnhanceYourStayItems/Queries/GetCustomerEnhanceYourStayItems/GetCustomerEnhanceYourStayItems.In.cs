namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItems;

public class GetCustomerEnhanceYourStayItemsIn
{
    public string? SearchValue { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int CustomerId { get; set; }
}
