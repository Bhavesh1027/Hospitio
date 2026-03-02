using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;

public class GetCustomerCurrencyOut : BaseResponseOut
{
    public GetCustomerCurrencyOut(string message, CustomerCurrencyByIdOut customerByIdOut) : base(message)
    {
        CustomerByIdOut = customerByIdOut;
    }
    public CustomerCurrencyByIdOut CustomerByIdOut { get; set; }
}
public class CustomerCurrencyByIdOut
{
    public string? CurrencyCode { get; set; }
}