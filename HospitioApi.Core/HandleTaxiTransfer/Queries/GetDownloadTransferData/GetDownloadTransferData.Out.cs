using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetDownloadTransferData;

public class GetDownloadTransferDataOut : BaseResponseOut
{
    public GetDownloadTransferDataOut(string message, string downloadTransferData) : base(message)
    {
        DownloadTransferData = downloadTransferData;
    }
    public string DownloadTransferData { get; set; }
}

public class DownloadTaxiTransferResponse
{
    public string? CustomerName { get; set; }
    public string? GuestFirstName { get; set; }
    public string? GuestLastName { get; set; }
    public string? TransferId { get; set; }
    public string? PickUpDateTime { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? MarkUpAmount { get; set; }
    public decimal? HospitioFareAmount { get; set; }
}