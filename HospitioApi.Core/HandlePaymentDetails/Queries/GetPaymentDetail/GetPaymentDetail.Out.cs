using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;

public class GetPaymentDetailOut : BaseResponseOut
{
    public GetPaymentDetailOut(string message, PaymentDetailOut payment) : base(message)
    {
        paymentDetailOut = payment;
    }
    public PaymentDetailOut paymentDetailOut { get; set; }
}
public class PaymentDetailOut
{
    public string? Token { get; set; }
    public string? Merchant_Account_Id { get; set; }
    public string? Buyer_Id { get; set; }
    public string? Country { get; set; }
}