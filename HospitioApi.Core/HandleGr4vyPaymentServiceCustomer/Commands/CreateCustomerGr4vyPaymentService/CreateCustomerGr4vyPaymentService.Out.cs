using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.CreateCustomerGr4vyPaymentService;

public class CreateCustomerGr4vyPaymentServiceOut : BaseResponseOut
{
    public CreateCustomerGr4vyPaymentServiceOut(string message, PaymentServiceOut payment) : base(message)
    {
        PaymentServiceOut = payment;
    }
    public PaymentServiceOut PaymentServiceOut { get; set; }
}
public class PaymentServiceOut
{
    public string? GRWebhookURL { get; set; }
}