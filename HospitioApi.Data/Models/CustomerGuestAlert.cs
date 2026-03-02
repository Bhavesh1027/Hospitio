namespace HospitioApi.Data.Models;

public partial class CustomerGuestAlert : Auditable
{
    public int? CustomerId { get; set; }
    public string? OfficeHoursMsg { get; set; }
    public int? OfficeHoursMsgWaitTimeInMinutes { get; set; }
    public string? OfflineHourMsg { get; set; }
    public int? OfflineHoursMsgWaitTimeInMinutes { get; set; }
    public bool? ReplyAtDiffPeriod { get; set; }

    public virtual Customer? Customer { get; set; }
}
