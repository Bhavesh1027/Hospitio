using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerMerchantAccount.Queries.GetCustomerMerchantAccount;

public class GetCustomerMerchantAccountOut : BaseResponseOut
{
    public GetCustomerMerchantAccountOut(string message, CustomerMerchantAccountOut customerCommunicationOut) : base(message)
    {
        CustomerCommunicationOut = customerCommunicationOut;
    }
    public CustomerMerchantAccountOut CustomerCommunicationOut { get; set; }
}
public class CustomerMerchantAccountOut
{
    public string? Type { get; set; }
    public string? Id { get; set; }
    public string? Display_Name { get; set; }
    public string? Outbound_Webhook_Url { get; set; }
    public string? Outbound_Webhook_Username { get; set; }
    public string? Outbound_Webhook_Password { get; set; }
    public string? Visa_Network_Tokens_Requestor_Id { get; set; }
    public string? Visa_Network_Tokens_App_Id { get; set; }
    public string? Amex_Network_Tokens_Requestor_Id { get; set; }
    public string? Amex_Network_Tokens_App_Id { get; set; }
    public string? Mastercard_Network_Tokens_Requestor_Id { get; set; }
    public string? Mastercard_Network_Tokens_App_Id { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Updated_At { get; set; }
}