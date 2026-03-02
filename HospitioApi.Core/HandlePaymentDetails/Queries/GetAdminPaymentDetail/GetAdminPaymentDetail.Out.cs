using HospitioApi.Shared;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetAdminPaymentDetail;

public class GetAdminPaymentDetailOut : BaseResponseOut
{
    public GetAdminPaymentDetailOut(string message , AdminPaymentDetailOut payment) : base(message)
    {
        adminPaymentDetailOut = payment;
    }
    public AdminPaymentDetailOut adminPaymentDetailOut {  get; set; }
}

public class AdminPaymentDetailOut
{
    public string? Token { get; set; }
    public string? Merchant_Account_Id { get; set; }
    public string? Buyer_Id { get; set; }
    public string? Country { get; set; }
}   
