using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentService;

public class GetCustomerGr4vyPaymentServiceOut : BaseResponseOut
{
    public GetCustomerGr4vyPaymentServiceOut(string message, List<CustomerPaymentServicesOut> paymentServicesOuts) : base(message)
    {
        customerPaymentServices = paymentServicesOuts;
    }
    public List<CustomerPaymentServicesOut> customerPaymentServices { get; set; } = new();
}
public class CustomerPaymentServicesOut
{
    public int? CustomerPaymentProcessorId { get; set; }
    public bool? IsActive { get; set; }
    public string? GRCategory { get; set; }
    public string? GRGroup { get; set; }
    public string? GRIcon { get; set; }
    public string? GRName { get; set; }

}