using HospitioApi.Shared;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;

public class UpdateAdminStaffAlertsOut : BaseResponseOut
{
    public UpdateAdminStaffAlertsOut(string message, UpdatedAdminStaffAlertsOut updatedAdminStaffAlertsOut) : base(message)
    {
        UpdatedAdminStaffAlertsOut = updatedAdminStaffAlertsOut;
    }
    public UpdatedAdminStaffAlertsOut UpdatedAdminStaffAlertsOut { get; set; }
}
public class UpdatedAdminStaffAlertsOut
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
    public string? VonageTemplateStatus { get; set; }
    public string? VonageTemplateId { get; set; }
    public string? WhatsappTemplateName { get; set; }
}