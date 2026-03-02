

namespace HospitioApi.Shared;
public static partial class GetPagedExtension
{
    public class BaseSearchFilterOptions
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public List<string> Sort { get; set; } = new();
        public string SearchParam { get; set; } = string.Empty;
        public string SearchColumn { get; set; } = string.Empty;
        public string SearchValue { get; set; } = string.Empty;
        public string SortColumn { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;

    }
}
