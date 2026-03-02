using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementData;

public class GetMusementDataOut : BaseResponseOut
{
    public GetMusementDataOut(string message, List<GetMusementDataResponseOut> getMusementDataResponse) : base(message)
    {
        getMusementDataResponseOut = getMusementDataResponse;
    }
    public List<GetMusementDataResponseOut>? getMusementDataResponseOut { get; set; }
}

public class GetMusementDataResponseOut
{
    public string? BusinessName { get; set; }
    public string? ReservationNumber { get;set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? OrderId { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? FilterCount { get; set; }
    public List<MusementItemnInfo>? MusementItemInfo { get; set; }
    public List<MusementItemnInfo>? MusementCancelItemInfo { get; set; }
}

public class MusementItemnInfo
{
    public string? Title { get; set; }
    public string? TicketHolder { get;set; }
    public bool? IsCancel { get; set; }
    public decimal? ItemPrice { get; set; }
}
