namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetAllTransferData;

public class GetAllTransferDataIn
{
    public string? SearchValue { get; set; }
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public int? CustomerId { get; set; }
    public int? GuestId { get; set; }
    public DateTime? FromCreateAt { get; set;}
    public DateTime? ToCreateAt { get; set; }
}
