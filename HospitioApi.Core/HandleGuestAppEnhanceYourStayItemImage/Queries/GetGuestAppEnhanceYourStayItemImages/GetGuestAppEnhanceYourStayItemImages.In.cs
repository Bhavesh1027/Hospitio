namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Queries.GetGuestAppEnhanceYourStayItemImages;

public class GetGuestAppEnhanceYourStayItemImagesIn
{
    public string? SearchColumn { get; set; }
    public string? SearchValue { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
}
