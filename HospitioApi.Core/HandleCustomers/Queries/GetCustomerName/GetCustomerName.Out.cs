using HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;

public class GetCustomerNameOut : BaseResponseOut
{
    public GetCustomerNameOut(string message, GetCustomerNameClass customerName) : base(message)
    {
        CustomerNameOut = customerName;
    }
    public GetCustomerNameClass CustomerNameOut { get; set; }
}
public class GetCustomerNameClass
{
    public string? BusinessName { get; set; }
}
