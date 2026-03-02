namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;

public class GetCustomersPaymentProcessorsIn
{
    public int CustomerId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}
