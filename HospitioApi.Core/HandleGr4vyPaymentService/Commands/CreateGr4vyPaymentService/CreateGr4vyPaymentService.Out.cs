using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.CreateGr4vyPaymentService;

public class CreateGr4vyPaymentServiceOut : BaseResponseOut
{
    public CreateGr4vyPaymentServiceOut(string message, PaymentServiceOut payment) : base(message)
    {
        PaymentServiceOut = payment;
    }
    public PaymentServiceOut PaymentServiceOut { get; set; }
}
public class PaymentServiceOut
{
    public string? GRWebhookURL { get; set; }
}