using Microsoft.Identity.Client;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Queries.GetAdminStaffAlerts;

public class GetAdminStaffAlertsOut : BaseResponseOut
{
    public GetAdminStaffAlertsOut(string message, List<AdminStaffAlertsOut> adminStaffAlertsOut) : base(message)
    {
        AdminStaffAlertsOut = adminStaffAlertsOut;
    }
    public List<AdminStaffAlertsOut> AdminStaffAlertsOut { get; set; } = new List<AdminStaffAlertsOut>();
}
public class AdminStaffAlertsOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Platfrom { get; set; } = string.Empty;
    public string PhoneCountry { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? WaitTimeInMintes { get; set; }
    public bool? IsActive { get; set; }
    public string? Msg { get; set; }
    public int? UserId { get; set; }
    public string WhatsappTemplateName {get;set;} = string.Empty;
    public string VonageTemplateStatus { get;set;} = string.Empty;
    public string VonageTemplateId { get;set;} = string.Empty;
}