namespace HospitioApi.Core.HandleAdminStaffAlerts.Commands.UpdateAdminStaffAlerts;

public class UpdateAdminStaffAlertsIn
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
}
