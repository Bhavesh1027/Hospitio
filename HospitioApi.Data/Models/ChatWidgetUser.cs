namespace HospitioApi.Data.Models;

public partial class ChatWidgetUser : Auditable
{
    public int? CustomerId { get; set; }
    public string? ChatWidgetUserToken { get; set; }
    public virtual Customer? Customer { get; set; }
}
