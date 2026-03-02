using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferMonths;

public class GetTaxiTransferMonthsOut : BaseResponseOut
{
    public GetTaxiTransferMonthsOut(string message, TaxiTransferMonths response) : base(message)
    {
        TaxiTransferMonths = response;
    }
    public TaxiTransferMonths TaxiTransferMonths { get; set; }
}
public class TaxiTransferMonths
{
    public List<int>? MonthNumber { get; set; }
}
