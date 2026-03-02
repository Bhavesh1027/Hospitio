using static HospitioApi.Shared.GetPagedExtension;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeads
{
    public class GetLeadsIn : BaseSearchFilterOptions
    {
        public string? AlphabetsStartsWith { get; set; }
    }
}
