using HospitioApi.Shared;

namespace HospitioApi.Core.HandleLeads.Queries.DownloadLead;

public class DownloadLeadOut : BaseResponseOut
{
    public DownloadLeadOut(string message, string downloadLeadByOut) : base(message)
    {
        DownloadLeadByOut = downloadLeadByOut;
    }
    public string DownloadLeadByOut { get;set; }
}
public class DownloadLeadByOut
{
    public string? Name { get; set; }
    public string? Company { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Comment { get; set; }

}