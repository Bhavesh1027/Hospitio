using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.UpdateGr4vyPaymentService;

public class UpdateGr4vyPaymentServiceOut : BaseResponseOut
{
    public UpdateGr4vyPaymentServiceOut(string message, PaymentServiceOut payment) : base(message)
    {
        PaymentServiceOut = payment;
    }
    public PaymentServiceOut PaymentServiceOut { get; set; }
}
public class PaymentServiceOut
{
    public string? GRWebhookURL { get; set; }
}