namespace HospitioApi.Data.Models;

public class AdminCustomerAlert : Auditable
{
    public string? Msg { get; set; }
    public int? MsgWaitTimeInMinutes { get; set; }

}
