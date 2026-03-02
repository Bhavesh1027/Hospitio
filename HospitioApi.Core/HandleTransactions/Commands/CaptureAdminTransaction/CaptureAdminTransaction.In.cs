namespace HospitioApi.Core.HandleTransactions.Commands.CaptureAdminTransaction;

public class CaptureAdminTransactionIn
{
    public int? CustomerId { get; set; }
    public string? Transaction_Id { get; set; }
    public int? Amount { get; set; }
}
