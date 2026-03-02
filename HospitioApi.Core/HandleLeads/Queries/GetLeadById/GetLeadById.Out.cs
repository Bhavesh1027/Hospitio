using HospitioApi.Shared;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeadById;

public class GetLeadByIdOut : BaseResponseOut
{
    public GetLeadByIdOut(string message, LeadByIdOut leadByIdOut) : base(message)
    {
        LeadByIdOut = leadByIdOut;
    }
    public LeadByIdOut LeadByIdOut { get; set; }
}
public class LeadByIdOut
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string? Comment { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? ContactFor { get; set; }
    public bool? IsActive { get; set; }
}
