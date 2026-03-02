using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.UpdateCustomerGr4vyPaymentService;

public class UpdateCustomerGr4vyPaymentServiceOut : BaseResponseOut
{
    public UpdateCustomerGr4vyPaymentServiceOut(string message, PaymentServiceOut payment) : base(message)
    {
        PaymentServiceOut = payment;
    }
    public PaymentServiceOut PaymentServiceOut { get; set; }
}
public class PaymentServiceOut
{
    public string? GRWebhookURL { get; set; }
}