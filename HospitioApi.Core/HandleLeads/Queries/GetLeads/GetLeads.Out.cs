using HospitioApi.Shared;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeads;

public class GetLeadsOut : BaseResponseOut
{
    public GetLeadsOut(string message, List<LeadsOut> leadsOuts) : base(message)
    {
        LeadsOuts = leadsOuts;
    }
    public List<LeadsOut> LeadsOuts { get; set; } = new();

    public class LeadsOut
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Comment { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ContactFor { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? Company { get; set; }
        public string? FilteredCount { get; set; }

    }
}