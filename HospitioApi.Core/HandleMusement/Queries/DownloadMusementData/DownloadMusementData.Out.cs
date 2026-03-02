using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Queries.DownloadMusementData;

public class DownloadMusementDataOut : BaseResponseOut
{
    public DownloadMusementDataOut(string message, List<DownloadMusementData> downloadMusementDataResponse) : base(message)
    {
        downloadMusementData = downloadMusementDataResponse;
    }
    public List<DownloadMusementData>? downloadMusementData { get; set; }
}
public class DownloadMusementData
{
    public string? BusinessName { get; set; }
    public string? ReservationNumber { get; set; }
    public string? City { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? OrderId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public List<MusementItemnInfo>? MusementItemInfo { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public List<MusementItemnInfo>? MusementCancelItemInfo { get; set; }
}

public class MusementItemnInfo
{
    public string? Title { get; set; }
}
