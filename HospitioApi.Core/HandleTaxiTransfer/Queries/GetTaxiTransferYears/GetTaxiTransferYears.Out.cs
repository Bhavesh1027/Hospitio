using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferYears;

public class GetTaxiTransferYearsOut : BaseResponseOut
{
    public GetTaxiTransferYearsOut(string message, TaxiTransferYears response) : base(message)
    {
        TaxiTransferYears = response;
    }
    public TaxiTransferYears TaxiTransferYears { get; set; }
}
public class TaxiTransferYears
{
    public List<int>? Years { get; set; }
}