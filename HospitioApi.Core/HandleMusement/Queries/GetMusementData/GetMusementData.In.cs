namespace HospitioApi.Core.HandleMusement.Queries.GetMusementData;

public class GetMusementDataIn
{
    public string? SearchValue { get; set; }
    public int? PageNo { get; set; }
    public int? PageSize { get; set;}
}
